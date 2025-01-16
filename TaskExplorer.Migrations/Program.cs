using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaskExplorer.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(
        rb => rb
            .AddPostgres()
            .WithGlobalConnectionString("Server=localhost;Port=5432;Database=jjj;Username=postgres;Password=pwd;")
            .ScanIn(typeof(AddUsersMigration).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.Run();
