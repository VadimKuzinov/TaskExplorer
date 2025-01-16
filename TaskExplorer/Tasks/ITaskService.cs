namespace TaskExplorer.Tasks;

public interface ITaskService
{
    Task CreateTask(TaskModel taskModel, CancellationToken cancellationToken);
    void UpdateTask(TaskNumber number, TaskModel taskModel, CancellationToken cancellationToken);
    void DeleteTask(TaskNumber number);
    TaskModelWithNumber[] GetTasksByAuthor(string author, CancellationToken cancellationToken);
    TaskModelWithNumber[] GetTasksByProject(string projectTitle, CancellationToken cancellationToken);
    TaskModelWithNumber? GetTaskByNumber(TaskNumber number, CancellationToken cancellationToken);
}
