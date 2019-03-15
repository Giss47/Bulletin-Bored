using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace DbAdapter
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<PostCategory> PostCategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer(@"Data Source=den1.mssql7.gear.host;Initial Catalog=bulletinbored; User ID=bulletinbored;Password=Ad6krxxu!~o9")
            //                     .UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));

            options.UseSqlServer(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=BulletinBored;Integrated Security=True")
                                 .UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
            .Property(p => p.Date)
            .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<PostCategory>()
            .HasKey(pc => new { pc.PostId, pc.CategoryId });

        }
    }
}
