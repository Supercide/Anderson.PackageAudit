using System.Threading.Tasks;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenant/{tenant}/projects")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]GetProjectsPipeline pipeline)
        {
            var context = new Context();
            await pipeline.HandleAsync(req, context);
            if (!context.HasError)
            {
                return new OkObjectResult(context.GetResponse<object>());
            }

            return null;
            //return response.Error.ToActionResult(errorResolver.Resolve);
        }
    }
}
