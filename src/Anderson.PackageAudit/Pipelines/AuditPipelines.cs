using Anderson.PackageAudit.Authorization;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Factories;
using Anderson.PackageAudit.NoOp;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Pipelines;

namespace Anderson.PackageAudit.Pipelines
{
    public class AuditPipelines
    {
        public static IRequestHandler<HttpRequest, Response<Unit, Error>> AuditPackages => 
            PipelineDefinitionBuilder<HttpRequest, Response<Unit, Error>>
                .StartWith(new AuthorizationHandler<Unit>(TokenValidationParametersFactory.Instance))
                .ThenWith(new NoOpHandler<HttpRequest, Response<Unit, Error>>(Unit.Instance))
                .Build();
    }
}
