namespace TaskExplorer.Api.Models;

public sealed record AuthenticateResp(int Id, string FirstName, string LastName, string UserName, string Token);
