using ServiceStack.Redis.Generic;

namespace Anderson.PackageAudit.Infrastructure.Persistence.Redis
{
    public interface IRedisClientFactory<T>
    {
        IRedisTypedClient<T> Instance { get; }
    }
}