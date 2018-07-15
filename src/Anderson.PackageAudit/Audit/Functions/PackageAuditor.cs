using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors.Extensions;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Audit.Functions
{
    public static class PackageAuditor
    {
        [FunctionName("PackageAuditor")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "packages")]HttpRequest req,
            [Inject]IErrorResolver<AuditError, IActionResult> auditErrorResolver,
            [Inject]IPackagePipelines packagePipelines)
        {
            var pipeline = packagePipelines.AuditPackages;
            var response = pipeline.Handle(req);
            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Success);
            }

            return response.Error.ToActionResult(auditErrorResolver.Resolve);
        }
    }
}
