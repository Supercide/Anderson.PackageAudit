using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Enrolment.Pipelines
{
    public interface EnrolmentPipeline : IRequestHandler<HttpRequest, Response<TenantResponse, Error>>
    {
    }
}