using TaskExplorer.Tasks;

namespace TaskExplorer.Api.Models;

public sealed record GetTasksByProjectResp(TaskModelWithNumber[] Tasks);