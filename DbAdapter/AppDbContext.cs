using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace DbAdapter
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=den1.mssql7.gear.host;Initial Catalog=bulletinbored; User ID=bulletinbored;Password=Ad6krxxu!~o9")
                                 .UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
        }
    }
}
