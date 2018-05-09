using Bizfitech.Banking.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Configurations
{
    internal class BankApiClientConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<BankApiClientConfiguration>
    {
        public void Configure(EntityTypeBuilder<BankApiClientConfiguration> builder)
        {
            builder.Property(p => p.ApiHost).IsRequired();
            builder.Property(p => p.AssemblyFullName).IsRequired();
            builder.Property(p => p.TypeName).IsRequired();
        }
    }
}
