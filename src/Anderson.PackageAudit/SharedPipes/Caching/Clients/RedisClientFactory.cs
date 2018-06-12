using System;
using Anderson.PackageAudit.Factories;
using ServiceStack.Redis;

namespace Anderson.PackageAudit.SharedPipes.Caching.Clients
{
    public class RedisClientFactory
    {
        static readonly Lazy<IRedisClient> _redisClient = new Lazy<IRedisClient>(() => {
            var redisManager = new RedisManagerPool(ConfigurationFactory.Instance["redis:connectionstring"]);
            return redisManager.GetClient();
        });

        public static IRedisClient Instance => _redisClient.Value;
    }
}