using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Infrastructure.Models
{
    public class AccountManagerDbContext : DbContext
    {
        public AccountManagerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AccountDbo> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var i = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
