using Bizfitech.Banking.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Configurations
{
    internal class BankEntityTypeConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
