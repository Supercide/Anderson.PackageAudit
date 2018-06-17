using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Anderson.PackageAudit
{
    public static class PackageAuditor
    {
        [FunctionName("PackageAuditor")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "packages")]HttpRequest req, [Inject]IPackagePipelines packagePipelines)
        {
            
            var pipeline = packagePipelines.AuditPackages;
            var response = pipeline.Handle(req);
            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Success);
            }

            return response.Error.ToActionResult(ErrorResolver.PackageAuditErrors);
        }

        [FunctionName("KeyGeneration")]
        public static IActionResult GenerateKey(
            [HttpTrigger(AuthorizationLevel.Anonymous, new []{ "post", "get" }, Route = "keys")]HttpRequest req,
            [Inject]IConfiguration config)
        {
            return new OkResult();
        }
    }
}
