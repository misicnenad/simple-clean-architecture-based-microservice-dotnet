using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace UserManager.Infrastructure.Models
{
    public class UserManagerDbContext : DbContext
    {
        public UserManagerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<UserDbo> Users { get; set; }
        public DbSet<AccountDbo> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
