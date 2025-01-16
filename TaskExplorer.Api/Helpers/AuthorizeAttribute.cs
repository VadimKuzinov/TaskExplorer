using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskExplorer.Api.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        context.HttpContext.Items.TryGetValue("User", out var user);
        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Unauthorized Access !!!" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
