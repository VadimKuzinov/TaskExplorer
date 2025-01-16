using TaskExplorer.Tasks;

namespace TaskExplorer.Api.Models;

public sealed record GetTasksByAuthorResp(TaskModelWithNumber[] Tasks);
