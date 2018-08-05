using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;

namespace Anderson.PackageAudit.SharedPipes.Authorization.Errors
{
    public class AuthorizationErrors : Error
    {
        public static AuthorizationErrors Unauthorized = new AuthorizationErrors(WellKnownErrorCodes.UnAuthorized, "Failed authorization");

        public AuthorizationErrors(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}
