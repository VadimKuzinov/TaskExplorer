using Microsoft.EntityFrameworkCore;

namespace TaskExplorer.DAL;

public class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : DbContext(options)
{
    public DbSet<ProjectDto> Projects { get; set; }
}

public sealed class ProjectDto
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Title { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
