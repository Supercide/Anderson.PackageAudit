using System.Linq;
using System.Xml.Schema;
using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Errors.Extensions;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Users.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Primitives;

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

        [FunctionName("RetriveTenant")]
        public static IActionResult GetTenant(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "tenants")]HttpRequest req,
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