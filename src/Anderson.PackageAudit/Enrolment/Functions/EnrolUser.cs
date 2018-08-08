using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Enrolment.Errors;
using Anderson.PackageAudit.Enrolment.Pipelines;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Enrolment.Functions
{
    public class EnrolUser
    {
        [FunctionName(nameof(EnrolUser))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "enrolment")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]EnrolmentPipeline pipeline)
        {
            var context = new Context();
            await pipeline.HandleAsync(req, context);

            if (context.HasError)
            {
                return context.GetError<EnrolmentError>().ToActionResult(errorResolver.Resolve);
            }

            return new OkObjectResult(context.GetResponse<TenantResponse>());
        }
    }
}