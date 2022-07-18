using JoggingApp.Core.Jogs;
using JoggingApp.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace JoggingApp.EntityFramework
{
    public class JoggingAppDbContext : DbContext
    {
        public JoggingAppDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Jog> Jogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        }
    }
}
