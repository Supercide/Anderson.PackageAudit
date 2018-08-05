using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Tenants.Pipelines
{
    public interface CreateTenantPipeline : IRequestHandler<HttpRequest, Response<Unit, Error>>
    {
    }
}