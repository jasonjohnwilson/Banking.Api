using Bizfitech.Banking.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Configurations
{
    internal class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(p => p.AccountNumber).IsRequired();
            builder.HasIndex(p => p.AccountNumber).IsUnique();
            builder.Property(p => p.BankId).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.HasOne(p => p.Bank).WithMany(p=>p.Accounts);
            builder.HasOne(p => p.User).WithMany(p => p.Accounts);
        }
    }
}
