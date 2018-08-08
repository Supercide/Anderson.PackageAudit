using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Projects.Errors;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.Projects.Pipelines;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Projects.Functions
{
    public class GetProjects
    {
        [FunctionName(nameof(GetProjects))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenants/{tenant}/projects")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]GetProjectsPipeline pipeline,
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

            return new OkObjectResult(context.GetResponse<IEnumerable<ProjectResponse>>());
        }
    }
}
