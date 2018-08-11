using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Packages.Functions;
using Anderson.PackageAudit.Vulnerabilities.Models;
using Anderson.PackageAudit.Vulnerabilities.Pipelines;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Vulnerabilities.Functions
{
    public class GetVulnerabilities
    {
        [FunctionName(nameof(GetVulnerabilities))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenants/{tenant}/vulnerabilities")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]GetVulnerabilitiesPipeline pipeline,
            [FromRoute]string tenant)
        {
            var context = new Context
            {
                ["tenant"]= tenant
            };
            await pipeline.HandleAsync(req, context);

            if (context.HasError)
            {
                return context.GetAllErrors().Cast<Error>().First().ToActionResult(errorResolver.Resolve);
            }

            return new OkObjectResult(context.GetResponse<IEnumerable<VulnerabilityResponse>>());
        }
    }
}
