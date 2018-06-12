using System.Collections.Generic;
using System.Linq;
using Anderson.PackageAudit.Errors;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Anderson.PackageAudit.SharedPipes.Caching
{
    public class CachingPipe<TRequest, TResponse> : PipelineDefinition<IList<TRequest>, Response<IList<TResponse>, Error>> 
        where TResponse : ICachableEntity
        where TRequest : ICachableEntity
    {
        private readonly IRedisTypedClient<TResponse> _redisClient;

        public CachingPipe(IRedisClient redisClient)
        {
            _redisClient = redisClient.As<TResponse>();
        }

        public override Response<IList<TResponse>, Error> Handle(IList<TRequest> request)
        {
            IList<TResponse> cachedPackages = _redisClient.GetByIds(request.Select(x => x.Id));

            var newPackages = request.Where(x => cachedPackages.All(package => package.Id != x.Id))
                                     .ToList();

            var response = InnerHandler.Handle(newPackages);

            if (response.IsSuccess)
            {
                _redisClient.StoreAll(response.Success);

                return response.Success
                               .Union(cachedPackages)
                               .ToList();
            }

            return response;
        }
    }
}
