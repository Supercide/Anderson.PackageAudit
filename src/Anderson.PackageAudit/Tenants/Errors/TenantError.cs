using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Tenants.Errors
{
    public class TenantError : Error
    {
        public TenantError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}