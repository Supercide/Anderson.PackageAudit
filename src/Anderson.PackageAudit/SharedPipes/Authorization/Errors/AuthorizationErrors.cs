using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;

namespace Anderson.PackageAudit.SharedPipes.Authorization.Errors
{
    public class AuthorizationErrors : Error
    {
        public static AuthorizationErrors Unauthorized = new AuthorizationErrors(WellKnownAuthorizationErrorCodes.UnAuthorized, "Failed authorization");

        public AuthorizationErrors(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}
