using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Tenants.Pipelines;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Tenants.Functions
{
    public class CreateTenant
    {
        [FunctionName(nameof(CreateTenant))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "tenants")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]CreateTenantPipeline pipeline)
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