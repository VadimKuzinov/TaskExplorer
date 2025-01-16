using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TaskExplorer.DAL;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public async Task<User?> GetUserById(int Id) => await Users.FindAsync(Id);

    public async Task<User?> Authenticate(string username, string password)
    {
        var user = await Users.SingleOrDefaultAsync(x => x.UserName == username && x.Password == password);
        if (user is not null) user.Password = "";
        return user;
    }
}

public sealed class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
}
