using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bizfitech.Banking.Api.Web.DependencyInjection
{
    public static class ApiClientServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClientProvider(this IServiceCollection services, IEnumerable<BankApiClientConfiguration> bankApiClientConfigurations)
        {
            var bankApisClients = new Dictionary<string, BankApiClientBase>();

            foreach(var bankApiClientConfiguration in bankApiClientConfigurations)
            {
                var assembly = Assembly.Load(bankApiClientConfiguration.AssemblyFullName);
                var type = assembly.GetType(bankApiClientConfiguration.TypeName);
                var client = Activator.CreateInstance(type, bankApiClientConfiguration) as BankApiClientBase;

                if(client == null)
                {
                    throw new Exception($"Client failed to create for BankApiConfiguration with ID {bankApiClientConfiguration.Id}");
                }
                
                bankApisClients.Add(bankApiClientConfiguration.Bank.Name, client);
            }
            
            var bankApiClientProvider = new BankApiClientProvider(bankApisClients);
            services.AddSingleton<IBankApiClientProvider>(bankApiClientProvider);
            
            return services;
        }
    }
}
