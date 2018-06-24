using System.Web.Http;
using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Users.Errors
{
    public class UserErrorResolver : IErrorResolver<UserError>
    {
        public IActionResult Resolve(Error error)
        {
            switch (error.ErrorCode)
            {
                case WellKnownAuthorizationErrorCodes.UnAuthorized:
                    return new UnauthorizedResult();
                case WellKnownUserErrors.UserNotFound:
                        return new NotFoundResult();
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}