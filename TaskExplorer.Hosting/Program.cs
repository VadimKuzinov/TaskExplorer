using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskExplorer.Api;
using TaskExplorer.Api.Helpers;
using TaskExplorer.Bll.Services.Tasks;
using TaskExplorer.DAL;
using TaskExplorer.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Настройка контекста базы данных
builder.Services
    .AddDbContext<UserDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddDbContext<TaskDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddDbContext<ProjectDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddCors();
// Добавление контроллеров
builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(AuthController).Assembly)
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Explorer API", Version = "v1" });
        c.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
    });

builder.Services
    .AddScoped<ITaskService, TaskService>()
    .AddScoped<ITaskStatusService, TaskStatusService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Explorer API v1"));
}

// global cors policy
app.UseCors(
    x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseMiddleware<JwtMiddleware>();

// Использование аутентификации
app.UseAuthentication();
app.UseAuthorization();

// Регистрация маршрутов для контроллеров
app.MapControllers();

app.Run();
