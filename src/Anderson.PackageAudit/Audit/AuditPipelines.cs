using System.Collections.Generic;
using Anderson.PackageAudit.Authorization;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Factories;
using Anderson.PackageAudit.NoOp;
using Anderson.PackageAudit.SharedPipes.Caching;
using Anderson.PackageAudit.SharedPipes.Caching.Clients;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Anderson.PackageAudit.Pipelines
{
    public class AuditPipelines
    {
        public static IRequestHandler<HttpRequest, Response<IList<Package>, Error>> AuditPackages => 

            PipelineDefinitionBuilder<HttpRequest, Response<IList<Package>, Error>>
                .StartWith(new AuthorizationHandler<IList<Package>>(TokenValidationParametersFactory.Instance))
                .ThenWithMutation(new HttpRequestMutation<IList<PackageRequest>, Response<IList<Package>, Error>>())
                .ThenWith(new CachingPipe<PackageRequest, Package>(RedisClientFactory.Instance))
                .ThenWith(new NoOpHandler<IList<PackageRequest>, Response<IList<Package>, Error>>(new List<Package>()))
                .Build();
    }
}
