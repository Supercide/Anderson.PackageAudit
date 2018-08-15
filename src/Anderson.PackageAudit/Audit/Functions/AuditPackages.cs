using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anderson.PackageAudit.Audit.Pipelines;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors.Extensions;
using Anderson.PackageAudit.Keys.Functions;
using Anderson.PackageAudit.Vulnerabilities.Models;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Audit.Functions
{
    public class AuditPackages
    {
        [FunctionName(nameof(AuditPackages))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "audit")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]AuditPackagePipeline pipeline)
        {
            var context = new Context();
            await pipeline.HandleAsync(req, context);

            if (context.HasError)
            {

                return context.GetAllErrors().Cast<Error>().First().ToActionResult(errorResolver.Resolve);
            }

            return new OkObjectResult(context.GetResponse<IList<Vulnerability>>());
        }
    }
}
