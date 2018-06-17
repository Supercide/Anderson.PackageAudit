using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection
{
    public class ConfigurationModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build());
        }
    }
}