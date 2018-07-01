using System.Web.Http;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Audit.Errors
{
    public class AuditErrorResolver : IErrorResolver<AuditError, IActionResult>
    {
        public IActionResult Resolve(Error error)
        {
            switch (error.ErrorCode)
            {
                case WellKnownAuthorizationErrorCodes.UnAuthorized:
                    return new UnauthorizedResult();
                case WellKnownAuditErrors.OSSIndexUnavailable:
                        return new ObjectResult("Unable to communicate with ossindex");
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}