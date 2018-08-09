using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Tenants.Models;
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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenants")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]GetTenantsPipeline pipeline)
        {
            var context = new Context();
            await pipeline.HandleAsync(req, context);

            if (context.HasError)
            {
                return context.GetAllErrors().Cast<Error>().First().ToActionResult(errorResolver.Resolve);
            }

            return new OkObjectResult(context.GetResponse<IEnumerable<TenantResponse>>());
        }
    }
}
