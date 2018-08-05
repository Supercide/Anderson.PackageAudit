using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Errors.Extensions;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Users.Functions
{
    public class TenantFunction
    {
        [FunctionName("RetriveTenant")]
        public static IActionResult GetTenant(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenants/{tenantName}")]HttpRequest req,
            string tenantName,
            [Inject]ITenantPipelines pipelines)
        {
           
            //TODO refactor this
            var pipeline = pipelines.RetrieveTenant;
            var response = pipeline.Handle(req);
            if (response.IsSuccess)
            {

                return new OkObjectResult(response.Success);
            }

            return new NotFoundObjectResult(response.Error.ErrorMessage);
        }
    }

    public class UserFunctions
    {
        [FunctionName("RetriveUserDetails")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "users")]HttpRequest req, 
            [Inject]IErrorResolver errorResolver,
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
            [Inject]IErrorResolver errorResolver,
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