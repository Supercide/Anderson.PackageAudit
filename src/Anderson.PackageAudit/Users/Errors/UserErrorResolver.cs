using System.Web.Http;
using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Users.Errors
{
    public class UserErrorResolver : IErrorResolver<UserError>
    {
        public IActionResult Resolve(Error error)
        {
            switch (error)
            {
                case AuthorizationErrors authorizationErrors:
                    return new UnauthorizedResult();
                case UserError userError:
                    return new BadRequestErrorMessageResult(userError.ErrorMessage);
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}