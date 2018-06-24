using ServiceStack.Redis.Generic;

namespace Anderson.PackageAudit.SharedPipes.Caching.Redis
{
    public interface IRedisClientFactory<T>
    {
        IRedisTypedClient<T> Instance { get; }
    }
}