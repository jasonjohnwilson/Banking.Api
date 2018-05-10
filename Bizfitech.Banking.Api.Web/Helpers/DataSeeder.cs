using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Bizfitech.Banking.Api.Web.Helpers
{
    public static class DataSeeder
    {
        public static void SeedData(BankContext context)
        {
            if (!context.Database.EnsureCreated())
            {
                context.Database.Migrate();
            }

            var fairwayBankGuid = Guid.NewGuid();

            if (context.Banks.Any() == false)
            {
                context.Banks.AddRange(new Bank[] {
                    new Bank{ Id = fairwayBankGuid, Name = "Fairway" }
                });
            }
            //todo: multiple clients using same plug in approach, only Fairway currently but others follow same approach
            //      also would need to copy dll into bin directory of wep app
            if (context.BankApiClientConfigurations.Any() == false)
            {
                context.BankApiClientConfigurations.AddRange(new BankApiClientConfiguration[] {
                    new BankApiClientConfiguration
                        {
                            Id = Guid.NewGuid(),
                            BankId = fairwayBankGuid,
                            ApiHost = "http://fairwaybank-bizfitech.azurewebsites.net/",
                            TypeName = "Bizfitech.Banking.Api.Client.FairwayBank.FairwayBankApiClient",
                            AssemblyFullName = "Bizfitech.Banking.Api.Client.FairwayBank"
                        }
                });
            }

            context.SaveChanges();
        }
    }
}
