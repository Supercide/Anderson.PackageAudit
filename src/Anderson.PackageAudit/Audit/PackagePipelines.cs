using System;
using System.Collections.Generic;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.SharedPipes.Authorization;
using Anderson.PackageAudit.SharedPipes.Caching;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Audit
{
    public class PackagePipelines : IPackagePipelines
    {
        private readonly PipelineDefinitionBuilder<HttpRequest, Response<IList<Package>, Error>> _builder;

        private readonly Lazy<IRequestHandler<HttpRequest, Response<IList<Package>, Error>>> _auditPipeline;

        public IRequestHandler<HttpRequest, Response<IList<Package>, Error>> AuditPackages => _auditPipeline.Value; 

        public PackagePipelines(PipelineDefinitionBuilder<HttpRequest, Response<IList<Package>, Error>> builder)
        {
            _builder = builder;
            _auditPipeline = new Lazy<IRequestHandler<HttpRequest, Response<IList<Package>, Error>>>(CreateAuditPackagePipeline);
        }

        private Func<IRequestHandler<HttpRequest, Response<IList<Package>, Error>>> CreateAuditPackagePipeline => 

            () => _builder.StartWith<AuthorizationPipe<IList<Package>>>()
                .ThenWithMutation<HttpRequestMutationPipe<IList<PackageRequest>, Response<IList<Package>, Error>>, IList<PackageRequest>>()
                .ThenWith<CachingPipe<PackageRequest, Package>>()
                .ThenWith<OSSIndexPipe>()
                .Build();
    }
}