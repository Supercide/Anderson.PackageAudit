using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Errors.Extensions;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Users.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Users.Functions
{
    public class UserFunctions
    {
        [FunctionName("RetriveUserDetails")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "users")]HttpRequest req, 
            [Inject]IErrorResolver<UserError, IActionResult> errorResolver,
            [Inject]IUserPipelines pipelines)
        {
            var pipeline = pipelines.RetrieveCurrentUser;
            var response = pipeline.Handle(req);
            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Success);
            }

            return response.Error.ToActionResult(errorResolver.Resolve);
        }

        [FunctionName("EnrolUser")]
        public static IActionResult Enrol(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "users")]HttpRequest req,
            [Inject]IErrorResolver<UserError, IActionResult> errorResolver,
            [Inject]IUserPipelines pipelines)
        {
            var response = pipelines.EnrolUser.Handle(req);
            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Success);
            }

            return response.Error.ToActionResult(errorResolver.Resolve);
        }
    }
}