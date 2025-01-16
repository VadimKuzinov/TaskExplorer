using TaskExplorer.Tasks;

namespace TaskExplorer.Bll.Services.Tasks;

public sealed class TaskStatusService : ITaskStatusService
{
    private static readonly Dictionary<TaskStatusModel, TaskStatusModel[]> s_statuses = new()
    {
        [TaskStatusModel.ToDo] = [TaskStatusModel.InProgress, TaskStatusModel.Closed],
        [TaskStatusModel.InProgress] = [TaskStatusModel.InReview, TaskStatusModel.ToDo, TaskStatusModel.Closed],
        [TaskStatusModel.InReview] = [TaskStatusModel.Testing, TaskStatusModel.InProgress, TaskStatusModel.Closed],
        [TaskStatusModel.Testing] = [TaskStatusModel.InProgress, TaskStatusModel.DevelopmentDone, TaskStatusModel.Closed],
        [TaskStatusModel.DevelopmentDone] = [TaskStatusModel.ReadyToDeploy, TaskStatusModel.Closed],
        [TaskStatusModel.ReadyToDeploy] = [TaskStatusModel.Closed],
        [TaskStatusModel.Closed] = [TaskStatusModel.ToDo],
    };

    public TaskStatusModel[] ListAvailableStatuses(TaskStatusModel status) => s_statuses.GetValueOrDefault(status, []);
    public TaskStatusModel[] ListStatuses() => Enum.GetValues(typeof(TaskStatusModel)).Cast<TaskStatusModel>().ToArray();
}
