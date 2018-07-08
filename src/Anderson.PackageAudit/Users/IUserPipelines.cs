using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Users
{
    public interface IUserPipelines
    {
        IRequestHandler<HttpRequest, Response<User, Error>> RetrieveCurrentUser { get; }
        IRequestHandler<HttpRequest, Response<User, Error>> EnrolUser { get; }
        IRequestHandler<HttpRequest, Response<Tenant, Error>> GetTenant { get; }
    }
}