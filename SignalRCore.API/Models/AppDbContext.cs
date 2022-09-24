using Microsoft.EntityFrameworkCore;

namespace SignalRCore.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Team> Team { get; set; }
    }
}
