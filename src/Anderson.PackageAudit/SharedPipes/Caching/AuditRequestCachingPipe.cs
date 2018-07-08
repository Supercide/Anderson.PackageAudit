using System.Collections.Generic;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Anderson.PackageAudit.SharedPipes.Caching
{
    public class AuditRequestCachingPipe : PipelineDefinition<AuditRequest, Response<AuditResponse, Error>> 
    {
        private readonly IRedisTypedClient<Package> _redisClient;

        public AuditRequestCachingPipe(IRedisClient redisClient)
        {
            _redisClient = redisClient.As<Package>();
        }

        public override Response<AuditResponse, Error> Handle(AuditRequest request)
        {
            IList<Package> cachedPackages = _redisClient.GetByIds(request.Packages.Select(x => x.Id));

            request.Packages = request.Packages
                                      .Where(x => cachedPackages.All(package => package.Id != x.Id))
                                      .ToList();

            var response = InnerHandler.Handle(request);

            if (response.IsSuccess)
            {
                CacheResponse(response.Success.Packages);

                return response.Success;
            }

            return response;
        }

        private void CacheResponse(IList<Package> response)
        {
            if(response.Any())
                _redisClient.StoreAll(response);
        }
    }
}
