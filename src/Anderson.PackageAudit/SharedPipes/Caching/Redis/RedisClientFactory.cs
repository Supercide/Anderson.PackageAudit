using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Anderson.PackageAudit.SharedPipes.Caching.Redis
{
    public class RedisClientFactory<T> : IRedisClientFactory<T>
    {
        public IRedisTypedClient<T> Instance { get; }
        public RedisClientFactory(IRedisClient redisClient)
        {
            Instance = redisClient.As<T>();
        }
    }
}