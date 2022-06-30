using Microsoft.EntityFrameworkCore;

namespace Atika.API
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Scoreboard> Scoreboard { get; set; }
    }
}
