using System.Web.Http;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Errors
{
    public class ErrorResolver
    {
        public static IActionResult PackageAuditErrors(Error error)
        {
            switch (error.ErrorCode)
            {
                case WellKnownErrorCodes.UnAuthorized:
                    return new UnauthorizedResult();
                case WellKnownErrorCodes.OSSIndexUnavailable:
                        return new ObjectResult("Unable to communicate with ossindex");
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}