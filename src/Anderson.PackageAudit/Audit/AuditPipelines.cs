using System.Collections.Generic;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.SharedPipes.Authorization;
using Anderson.PackageAudit.SharedPipes.Authorization.Factories;
using Anderson.PackageAudit.SharedPipes.Caching;
using Anderson.PackageAudit.SharedPipes.Caching.Clients;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Audit
{
    public class AuditPipelines
    {
        public static IRequestHandler<HttpRequest, Response<IList<Package>, Error>> AuditPackages => 

            PipelineDefinitionBuilder<HttpRequest, Response<IList<Package>, Error>>
                .StartWith(new AuthorizationHandler<IList<Package>>(TokenValidationParametersFactory.Instance))
                .ThenWithMutation(new HttpRequestMutation<IList<PackageRequest>, Response<IList<Package>, Error>>())
                .ThenWith(new CachingPipe<PackageRequest, Package>(RedisClientFactory.Instance))
                .ThenWith(new OSSIndexPipe(ConfigurationFactory.Instance))
                .Build();
    }
}
