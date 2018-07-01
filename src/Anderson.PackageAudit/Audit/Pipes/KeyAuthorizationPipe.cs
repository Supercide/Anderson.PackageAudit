using Anderson.PackageAudit.Core.Errors;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class KeyAuthorizationPipe<TSuccess> : PipelineDefinition<HttpRequest, Response<TSuccess, Error>>
    {
        public override Response<TSuccess, Error> Handle(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("X-API-KEY"))
            {
                return SharedPipes.Authorization.Errors.AuthorizationErrors.Unauthorized;
            }

            return InnerHandler.Handle(request);
        }
    }
}