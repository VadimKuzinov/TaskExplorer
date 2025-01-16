using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskExplorer.Api.Helpers;
using TaskExplorer.Api.Models;
using TaskExplorer.DAL;

namespace TaskExplorer.Api;

[ApiController]
[Route("[controller]")]
public class AuthController(UserDbContext context, IOptions<JwtSettings> jwtSettings) : ControllerBase
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterReq request)
    {
        var user = request.User;

        if (context.Users.Any(u => u.UserName == user.UserName))
            return BadRequest("Username is already taken");

        context.Users.Add(
            new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                UserName = user.UserName
            });

        await context.SaveChangesAsync();
        return Ok();
    }

    /*
    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);

        if (user == null || !VerifyPassword(password, user.Password))
            return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey("YourSuperSecretKey"u8.ToArray()), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { Token = tokenHandler.WriteToken(token) });
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        // Реализуйте проверку пароля здесь (например, с использованием хеширования)
        return password == passwordHash; // Упрощение для примера
    }*/

    [HttpPost("Authenticate")]
    public async Task<ActionResult<AuthenticateResp>> Authenticate(AuthenticateReq model)
    {
        var user = await context.Authenticate(model.UserName, model.Password);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        var token = Common.GenerateJwtToken(user.Id, _jwtSettings);

        return new AuthenticateResp(user.Id, user.FirstName, user.LastName, user.UserName, token);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto[]>> GetAll() =>
        await context.Users.Select(x => new UserDto(x.FirstName, x.LastName, x.UserName, x.Password)).ToArrayAsync();

    [Authorize]
    [HttpGet("info")]
    public ActionResult<UserDto> GerUserInfo()
    {
        var user = (User)HttpContext.Items["User"]!;
        return new UserDto(user.FirstName, user.LastName, user.UserName, "");
    }
}
