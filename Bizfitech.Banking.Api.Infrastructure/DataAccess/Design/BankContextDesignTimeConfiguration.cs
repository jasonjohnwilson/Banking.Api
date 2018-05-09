using Bizfitech.Banking.Api.Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Design
{
    public class BankContextDesignTimeConfiguration : IDesignTimeDbContextFactory<BankContext>
    {
        public BankContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNET_ENV");

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<BankContext>();
            dbContextOptionsBuilder.UseSqlServer(configuration["ConnectionStrings:BankDb"],
                options =>
                options.MigrationsAssembly(Assembly.GetAssembly(typeof(BankContextDesignTimeConfiguration)).GetName().Name));

            var context = new BankContext(dbContextOptionsBuilder.Options);

            return context;
        }
    }
}
