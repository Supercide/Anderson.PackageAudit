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
        private readonly IRedisTypedClient<PackageSummary> _redisClient;

        public AuditRequestCachingPipe(IRedisClient redisClient)
        {
            _redisClient = redisClient.As<PackageSummary>();
        }

        public override Response<AuditResponse, Error> Handle(AuditRequest request)
        {
            IList<PackageSummary> cachedPackages = _redisClient.GetByIds(request.Packages.Select(x => $"{x.Name}{x.Version}".ToUpper()));

            request.Packages = request.Packages
                                      .Where(x => cachedPackages.All(package => x.Name != package.Name && 
                                                                                x.Version != package.Version))
                                      .ToList();

            var response = InnerHandler.Handle(request);

            if (response.IsSuccess)
            {
                CacheResponse(response.Success.Packages);

                return response.Success;
            }

            return response;
        }

        private void CacheResponse(PackageSummary[] packages)
        {
            if(packages.Any())
                _redisClient.StoreAll(packages);
        }
    }
}
