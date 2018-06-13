using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit
{
    public static class PackageAuditor
    {
        [FunctionName("PackageAuditor")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "packages")]HttpRequest req)
        {
            var pipeline = AuditPipelines.AuditPackages;
            var response = pipeline.Handle(req);
            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Success);
            }

            return response.Error.ToActionResult(ErrorResolver.PackageAuditErrors);
        }
    }
}
