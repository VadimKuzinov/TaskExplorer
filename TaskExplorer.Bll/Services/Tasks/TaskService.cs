using TaskExplorer.DAL;
using TaskExplorer.Tasks;
using TaskExplorer.Tasks.Exceptions;

namespace TaskExplorer.Bll.Services.Tasks;

public sealed class TaskService(TaskDbContext taskDbContext, ProjectDbContext projectDbContext, ITaskStatusService taskStatusService)
    : ITaskService
{
    private const string DefaultTaskNumberPrefix = "UNKNOWN";

    public async Task CreateTask(TaskModel taskModel, CancellationToken cancellationToken)
    {
        if (taskModel.ProjectTitle is not null)
        {
            var projectId = projectDbContext.Projects.Where(x => x.Title == taskModel.ProjectTitle).Select(x => x.Id).FirstOrDefault();
            taskModel = taskModel with { ProjectId = projectId };
        }

        var taskModelWithNumber = new TaskModelWithNumber(GenerateTaskNumber(taskModel), taskModel);
        await taskDbContext.Tasks.AddAsync(MapToDto(taskModelWithNumber), cancellationToken);
        await taskDbContext.SaveChangesAsync(cancellationToken);
    }

    private TaskNumber GenerateTaskNumber(TaskModel taskModel)
    {
        var taskNumberPrefix = projectDbContext.Projects.Find(taskModel.ProjectId)?.Number ?? DefaultTaskNumberPrefix;

        var latestNumber = taskDbContext.Tasks.Where(x => x.NumberPrefix == taskNumberPrefix)
            .OrderByDescending(x => x.NumberValue)
            .FirstOrDefault()?.NumberValue ?? 0;

        return new(taskNumberPrefix, latestNumber + 1);
    }

    public void UpdateTask(TaskNumber number, TaskModel taskModel, CancellationToken cancellationToken)
    {
        var taskDb = FindByNumber(number);
        if (taskDb is null) return;

        var oldTaskModel = MapToBll(taskDb);
        var updateModel = GenerateUpdateModel(oldTaskModel.TaskModel, taskModel);
        var updateModelDto = MapToDto(oldTaskModel with { TaskModel = updateModel });
        updateModelDto.Id = taskDb.Id;

        taskDbContext.Entry(taskDb).CurrentValues.SetValues(updateModelDto);
        taskDbContext.SaveChanges();
    }

    public void DeleteTask(TaskNumber number)
    {
        var task = FindByNumber(number);
        if (task is not null)
        {
            taskDbContext.Tasks.Remove(task);
            taskDbContext.SaveChanges();
        }
    }

    public TaskModelWithNumber[] GetTasksByAuthor(string author, CancellationToken cancellationToken) =>
        taskDbContext.Tasks.Where(x => x.CreatorLogin == author).Select(x => MapToBll(x)).ToArray();

    public TaskModelWithNumber[] GetTasksByProject(string projectTitle, CancellationToken cancellationToken)
    {
        var projectId = projectDbContext.Projects.SingleOrDefault(x => x.Title == projectTitle)?.Id;
        return projectId is null ? [] : taskDbContext.Tasks.Where(x => x.ProjectId == projectId.Value).Select(x => MapToBll(x)).ToArray();
    }

    public TaskModelWithNumber? GetTaskByNumber(TaskNumber number, CancellationToken cancellationToken)
    {
        var taskDb = FindByNumber(number);
        return taskDb is null ? null : MapToBll(taskDb);
    }

    private TaskDto? FindByNumber(TaskNumber number) =>
        taskDbContext.Tasks.SingleOrDefault(t => t.NumberPrefix == number.Prefix && t.NumberValue == number.Number);

    private static TaskModelWithNumber MapToBll(TaskDto dto)
    {
        return new TaskModelWithNumber(
            new(dto.NumberPrefix, dto.NumberValue),
            new(
                dto.Title,
                dto.Description,
                dto.DueDate,
                dto.ProjectId,
                dto.SprintId,
                dto.CreatorLogin,
                dto.AssignedToLogin,
                dto.ReporterLogin,
                dto.ProjectTitle,
                (TaskStatusModel)dto.Status));
    }

    private static TaskDto MapToDto(TaskModelWithNumber model)
    {
        var now = DateTimeOffset.UtcNow;
        return new TaskDto
        {
            NumberPrefix = model.Number.Prefix,
            NumberValue = model.Number.Number,
            AssignedToLogin = model.TaskModel.AssignedToLogin,
            CreatorLogin = model.TaskModel.CreatorLogin,
            ReporterLogin = model.TaskModel.ReporterLogin,
            ProjectId = model.TaskModel.ProjectId,
            SprintId = model.TaskModel.SprintId,
            Title = model.TaskModel.Title,
            Description = model.TaskModel.Description,
            DueDate = model.TaskModel.DueDate,
            CreatedAt = now,
            UpdatedAt = now,
            ProjectTitle = model.TaskModel.ProjectTitle,
            Status = (int)model.TaskModel.Status
        };
    }

    private TaskModel GenerateUpdateModel(TaskModel oldTaskModel, TaskModel newTaskModel)
    {
        if (oldTaskModel.Status != newTaskModel.Status) ValidateNewStatus(oldTaskModel, newTaskModel);
        return oldTaskModel with
        {
            Description = newTaskModel.Description,
            DueDate = newTaskModel.DueDate,
            ReporterLogin = newTaskModel.ReporterLogin,
            Title = newTaskModel.Title,
            SprintId = newTaskModel.SprintId,
            AssignedToLogin = newTaskModel.AssignedToLogin,
            Status = newTaskModel.Status
        };
    }

    private void ValidateNewStatus(TaskModel oldTaskModel, TaskModel newTaskModel)
    {
        var availableStatuses = taskStatusService.ListAvailableStatuses(oldTaskModel.Status);
        if (!availableStatuses.Contains(newTaskModel.Status))
        {
            throw new IncorrectTaskStatusException("New status is incorrect.");
        }
    }
}
