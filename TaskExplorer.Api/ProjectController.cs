using Microsoft.AspNetCore.Mvc;
using TaskExplorer.Api.Helpers;
using TaskExplorer.Api.Models;
using TaskExplorer.DAL;
using TaskExplorer.Projects;
using TaskExplorer.Tasks;

namespace TaskExplorer.Api;

[ApiController]
[Route("[controller]")]
public sealed class ProjectController(ProjectDbContext dbContext) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateProject(CreateProjectReq request, CancellationToken cancellationToken)
    {
        await dbContext.Projects.AddAsync(MapToDto(request.Project), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    [HttpGet("list")]
    [Authorize]
    public ActionResult<ListProjectsResp> ListProjects() =>
        new ListProjectsResp(dbContext.Projects.Select(x => MapToBll(x)).ToArray());

    private static ProjectDto MapToDto(Project project) =>
        new()
            { Number = project.Number, Title = project.Title, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

    private static Project MapToBll(ProjectDto project) => new(project.Number, project.Title);
}
