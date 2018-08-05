using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public class ConfigurationModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            serviceCollection.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build());
        }
    }
}