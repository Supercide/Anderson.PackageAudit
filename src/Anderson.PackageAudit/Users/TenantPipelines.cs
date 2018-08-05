using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Users
{
    public class TenantPipelines : ITenantPipelines
    {
        private readonly PipelineDefinitionBuilder _builder;

        public TenantPipelines(PipelineDefinitionBuilder builder)
        {
            _builder = builder;
        }

        public IRequestHandler<HttpRequest, Response<TenantOverview, Error>> RetrieveTenant => _builder
            .StartWith<AuthorizationPipe<TenantOverview>, HttpRequest, Response<TenantOverview, Error>>()
            .ThenWith<RetrieveTenantPipe>()
            .Build();
    }
}