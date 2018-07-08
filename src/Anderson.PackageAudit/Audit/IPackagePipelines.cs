using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Audit
{
    public interface IPackagePipelines
    {
        IRequestHandler<HttpRequest, Response<AuditResponse, Error>> AuditPackages { get; }
    }
}