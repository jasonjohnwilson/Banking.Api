using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Contexts
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankApiClientConfiguration> BankApiClientConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<User>(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration<Account>(new AccountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration<Bank>(new BankEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration<BankApiClientConfiguration>(new BankApiClientConfigurationEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
