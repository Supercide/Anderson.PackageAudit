using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Errors;

namespace Anderson.PackageAudit.Users.Errors
{
    public class UserError : Error
    {
        public static readonly UserError NotFound = new UserError(WellKnownUserErrors.UserNotFound, "Usermust be enrolled");
        public static readonly UserError TenantNameTaken = new UserError(WellKnownUserErrors.TenantNameTaken, "Tenant name has been taken");

        protected UserError(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}