using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Tenants.Pipelines;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Tenants.Functions
{
    public class GetTenants
    {
        [FunctionName(nameof(GetTenants))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenants")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]GetTenantsPipeline pipeline)
        {
            var context = new Context();
            var response = pipeline.HandleAsync(req, context);
            if (!context.HasError)
            {
                return new OkObjectResult(context.GetResponse<object>());
            }

            return null;
            //return response.Error.ToActionResult(errorResolver.Resolve);
        }
    }
}
