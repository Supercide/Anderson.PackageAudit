using Anderson.PackageAudit.Core.Errors;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Users
{
    public interface ITenantPipelines
    {
        IRequestHandler<HttpRequest, Response<TenantOverview, Error>> RetrieveTenant { get; }
    }
}