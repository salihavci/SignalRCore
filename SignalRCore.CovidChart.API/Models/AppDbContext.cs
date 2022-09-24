using Microsoft.EntityFrameworkCore;

namespace SignalRCore.CovidChart.API.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Covid> Covid { get; set; }
    }
}
