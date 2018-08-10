using Autofac;
using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;

namespace Anderson.PackageAudit.Infrastructure.Persistence.Redis
{
    public class RedisModule: Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(provider =>
            {
                var configuration = provider.Resolve<IConfiguration>();
                return new RedisManagerPool(configuration["redis:connectionstring"]);
            }).InstancePerLifetimeScope().AsSelf();

            containerBuilder.Register(provider =>
            {
                var managerPool = provider.Resolve<RedisManagerPool>();
                return managerPool.GetClient();
            }).InstancePerLifetimeScope().AsSelf();

            containerBuilder.RegisterGeneric(typeof(RedisClientFactory<>)).As(typeof(RedisClientFactory<>)).InstancePerLifetimeScope();
        }
    }
}