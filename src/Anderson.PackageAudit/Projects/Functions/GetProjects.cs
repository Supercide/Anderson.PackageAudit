using System;
using System.Text;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Errors.Extensions;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Keys.Pipelines;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.Projects.Pipelines;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Projects.Functions
{
    public class GetProjects
    {
        [FunctionName(nameof(GetProjects))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenant/{tenant}/projects")]HttpRequest req,
            [Inject]IErrorResolver errorResolver,
            [Inject]GetProjectsPipeline pipeline)
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
