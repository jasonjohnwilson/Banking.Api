using Bizfitech.Banking.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Configurations
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName).IsRequired();
            builder.Property(p => p.LastName).IsRequired();
            builder.Property(p => p.Username).IsRequired();
            builder.HasIndex(p => p.Username).IsUnique();
            builder.HasMany(p => p.Accounts).WithOne();
        }
    }
}
