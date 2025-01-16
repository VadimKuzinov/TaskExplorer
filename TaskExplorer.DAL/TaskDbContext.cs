using Microsoft.EntityFrameworkCore;

namespace TaskExplorer.DAL;

public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<TaskDto> Tasks { get; set; }
}

public sealed class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public int? ProjectId { get; set; }
    public int? SprintId { get; set; }
    public string CreatorLogin { get; set; }
    public string? AssignedToLogin { get; set; }
    public string? ReporterLogin { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public string NumberPrefix { get; set; }
    public int NumberValue { get; set; }
    public int Status { get; set; }
    public string? ProjectTitle { get; set; }
}
