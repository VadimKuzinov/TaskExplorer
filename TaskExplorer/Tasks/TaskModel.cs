namespace TaskExplorer.Tasks;

public sealed record TaskModel(
    string Title,
    string Description,
    DateTimeOffset? DueDate,
    int? ProjectId,
    int? SprintId,
    string? CreatorLogin,
    string? AssignedToLogin,
    string? ReporterLogin,
    string? ProjectTitle,
    TaskStatusModel Status);
