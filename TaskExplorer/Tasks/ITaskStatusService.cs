namespace TaskExplorer.Tasks;

public interface ITaskStatusService
{
    public TaskStatusModel[] ListAvailableStatuses(TaskStatusModel status);
    public TaskStatusModel[] ListStatuses();
}
