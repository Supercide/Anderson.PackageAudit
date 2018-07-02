using System.Web.Http;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Users.Errors
{
    public class UserErrorResolver : IErrorResolver<UserError, IActionResult>
    {
        public IActionResult Resolve(Error error)
        {
            switch (error)
            {
                case AuthorizationErrors authorizationErrors:
                    return new UnauthorizedResult();
                case UserError userError:
                    return HandleUserError(userError);
                default:
                    return new InternalServerErrorResult();
            }
        }

        private static IActionResult HandleUserError(UserError userError)
        {
            switch (userError.ErrorCode)
            {
                case WellKnownUserErrors.UserNotFound:
                    return new NotFoundObjectResult(userError.ErrorMessage);
                case WellKnownUserErrors.UserAlreadyEnrolled:
                case WellKnownUserErrors.TenantNameTaken:
                    return new BadRequestErrorMessageResult(userError.ErrorMessage);
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}