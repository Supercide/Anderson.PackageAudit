using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Errors.Extensions;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Projects.Pipelines;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Projects.Functions
{
    public class AuditProject
    {
        [FunctionName(nameof(AuditProject))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "tenant/{tenant}/projects")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]AuditProjectPipeline pipeline)
        {
            var response = pipeline.Handle(req);
            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Success);
            }

            return response.Error.ToActionResult(errorResolver.Resolve);
        }
    }
}