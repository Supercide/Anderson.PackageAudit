using System.Web.Http;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Keys.Errors
{
    

    public class KeyErrorResolver : IErrorResolver<KeyError, IActionResult>
    {
        public IActionResult Resolve(Error error)
        {
            switch (error)
            {
                case KeyError keyError:
                    return new BadRequestObjectResult(keyError.ErrorMessage);
                case TenantError tenantError:
                    return new BadRequestObjectResult(tenantError.ErrorMessage);
                case AuthorizationErrors authError:
                    return new UnauthorizedResult();
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}
