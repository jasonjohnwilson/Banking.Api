using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Contexts;
using Bizfitech.Banking.Api.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bizfitech.Banking.Api.Web.Configuration
{
    internal class EntityFrameworkConfigurationProvider : ConfigurationProvider
    {
        public const string BankApiConfigurations = "BankApiClientConfigurations";

        private Action<DbContextOptionsBuilder> _optionsAction;

        public EntityFrameworkConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }
        
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<BankContext>();

            _optionsAction(builder);
            
            using (var dbContext = new BankContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();
                //todo: move to configure
                DataSeeder.SeedData(dbContext);

                Data = !dbContext.BankApiClientConfigurations.Any()
                    ? GetConfiguration(Enumerable.Empty<BankApiClientConfiguration>().AsQueryable())
                    : GetConfiguration(dbContext.BankApiClientConfigurations);
            }
        }

        private Dictionary<string,string> GetConfiguration(IQueryable<BankApiClientConfiguration> bankApiClientConfigurations)
        {
            var dictionary = new Dictionary<string, string>();
            var apiConfigurations = new List<BankApiClientConfiguration>();

            foreach (var bankApiClientConfiguration in bankApiClientConfigurations.Include(i => i.Bank))
            {
                apiConfigurations.Add(bankApiClientConfiguration);
            }

            dictionary.Add(BankApiConfigurations, JsonConvert.SerializeObject(apiConfigurations));

            return dictionary;
        }
    }
}