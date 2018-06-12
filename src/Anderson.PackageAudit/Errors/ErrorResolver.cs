using System.Web.Http;
using Anderson.PackageAudit.Authorization;
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
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}