using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;

namespace Anderson.PackageAudit.Infrastructure.Persistence.Redis
{
    public class RedisModule: ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new RedisManagerPool(configuration["redis:connectionstring"]);
            });

            serviceCollection.AddSingleton(provider =>
            {
                var managerPool = provider.GetService<RedisManagerPool>();
                return managerPool.GetClient();
            });

            serviceCollection.AddScoped(typeof(IRedisClientFactory<>), typeof(RedisClientFactory<>));
        }
    }
}