using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Vulnerabilities.Models;
using Anderson.PackageAudit.Vulnerabilities.Pipelines;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Vulnerabilities.Functions
{
    public class GetVulnerabilitiesForProject
    {
        [FunctionName(nameof(GetVulnerabilitiesForProject))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenants/{tenant}/projects/{project}/vulnerabilities")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]GetVulnerabilitiesPipeline pipeline,
            [FromRoute]string tenant,
            [FromRoute]string project
            )
        {
            var context = new Context
            {
                ["tenant"] = tenant,
                ["project"] = project
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
