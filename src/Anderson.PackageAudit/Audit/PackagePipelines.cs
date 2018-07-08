using System;
using System.Collections.Generic;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
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
        public PackagePipelines(PipelineDefinitionBuilder<HttpRequest, Response<AuditResponse, Error>> builder)
        {
            AuditPackages = builder.StartWith<AuthorizationPipe<AuditResponse>>()
                .ThenWith<KeyAuthorizationPipe<AuditResponse>>()
                .ThenWithMutation<AuditMutation, AuditRequest>()
                .ThenWith<AuditRequestCachingPipe>()
                .ThenWith<OSSIndexPipe>()
                .Build();
        }

        public IRequestHandler<HttpRequest, Response<AuditResponse, Error>> AuditPackages { get; }
    }
}