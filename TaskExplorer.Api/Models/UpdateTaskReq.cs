using TaskExplorer.Tasks;

namespace TaskExplorer.Api.Models;

public sealed record UpdateTaskReq(TaskNumber Number, TaskModel Task);
