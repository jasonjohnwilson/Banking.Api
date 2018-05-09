using Bizfitech.Banking.Api.Core.Factories;
using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Core.Models;
using Bizfitech.Banking.Api.Core.Services;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Contexts;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Repositories;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Uow;
using Bizfitech.Banking.Api.Web.Configuration;
using Bizfitech.Banking.Api.Web.DependencyInjection;
using Bizfitech.Banking.Api.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bizfitech.Banking.Api.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json")
                           .AddJsonFile($"appsettings.{_environment.EnvironmentName}.json", true)
                           .AddEnvironmentVariables();

            IConfigurationRoot configuration = configurationBuilder.Build();

            configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                            .AddJsonFile($"appsettings.{_environment.EnvironmentName}.json", true)
                            .AddEnvironmentVariables()
                            .AddEntityFrameworkConfig(
                                action => action.UseSqlServer(configuration.GetConnectionString("BankDb")));

            configuration = configurationBuilder.Build();
            
            var bankApiClientConfigurations = 
                JsonConvert.DeserializeObject<IEnumerable<BankApiClientConfiguration>>(
                configuration.GetValue<string>(EntityFrameworkConfigurationProvider.BankApiConfigurations));

            var connectionString = configuration.GetConnectionString("BankDb");

            services.AddApiClientProvider(bankApiClientConfigurations);
            services.AddDbContext<BankContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBankUow, BankUow>();
            services.AddTransient<IGenericRepository<User>, GenericRepository<User>>();
            services.AddSingleton<IFactory, SimpleFactory>();
            services.AddMvc();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Bizfitech Banking API", Version = "v1" });

                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Mappers.Initialise();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bizfitech Banking API V1");
            });
            
            app.UseMvc();
        }
    }
}
