using TaskExplorer.Tasks;

namespace TaskExplorer.Api.Models;

public sealed record GetTaskByNumberResp(TaskModelWithNumber? Task);
