using Microsoft.AspNetCore.Mvc;
using TaskExplorer.Api.Helpers;
using TaskExplorer.Api.Models;
using TaskExplorer.DAL;
using TaskExplorer.Tasks;
using TaskExplorer.Tasks.Exceptions;

namespace TaskExplorer.Api;

[ApiController]
[Route("[controller]")]
public sealed class TaskController(ITaskService taskService, ITaskStatusService taskStatusService) : ControllerBase
{
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTask(CreateTaskReq request, CancellationToken cancellationToken)
    {
        var user = (User)HttpContext.Items["User"]!;
        await taskService.CreateTask(request.TaskModel with { CreatorLogin = user.UserName }, cancellationToken);
        return Ok();
    }

    [HttpPost("list/by-author")]
    public ActionResult<GetTasksByAuthorResp> GetTasksByAuthor(GetTasksByAuthorReq request, CancellationToken cancellationToken) =>
        new GetTasksByAuthorResp(taskService.GetTasksByAuthor(request.Login, cancellationToken) ?? []);

    [HttpDelete("delete")]
    public IActionResult DeleteTask(DeleteTaskReq request)
    {
        taskService.DeleteTask(request.Number);
        return Ok();
    }

    [HttpPut("update")]
    public IActionResult UpdateTask(UpdateTaskReq request, CancellationToken cancellationToken)
    {
        try
        {
            taskService.UpdateTask(request.Number, request.Task, cancellationToken);
            return Ok();
        }
        catch (IncorrectTaskStatusException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("available-statuses")]
    public ActionResult<ListAvailableStatusesResp> ListAvailableStatuses(ListAvailableStatusesReq request) =>
        new ListAvailableStatusesResp(taskStatusService.ListAvailableStatuses(request.Status));

    [HttpGet("statuses")]
    public ActionResult<ListStatusesResp> ListStatuses() =>
        new ListStatusesResp(taskStatusService.ListStatuses());

    [HttpPost("by-project")]
    public ActionResult<GetTasksByProjectResp> GetTasksByProject(GetTasksByProjectReq request, CancellationToken cancellationToken) =>
        new GetTasksByProjectResp(taskService.GetTasksByProject(request.ProjectTitle, cancellationToken));

    [HttpPost("by-number")]
    public ActionResult<GetTaskByNumberResp> GetTaskByNumber(GetTaskByNumberReq request, CancellationToken cancellationToken) =>
        new GetTaskByNumberResp(taskService.GetTaskByNumber(request.Number, cancellationToken));
}
