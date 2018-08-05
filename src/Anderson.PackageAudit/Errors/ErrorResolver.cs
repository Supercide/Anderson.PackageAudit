using System.Web.Http;
using Anderson.PackageAudit.Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Errors
{

    public class ErrorResolver : IErrorResolver
    {
        public IActionResult Resolve(Error error)
        {
            switch (error.ErrorType)
            {
                case ErrorType.RequestError:
                    return new BadRequestObjectResult(error.ErrorMessage);
                case ErrorType.AuthenticationError:
                    return new UnauthorizedResult();
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}
