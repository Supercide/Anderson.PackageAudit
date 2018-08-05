using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Enrolment.Errors
{
    public class EnrolmentError : Error
    {
        public EnrolmentError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }

        public static EnrolmentError TenantNameInUse = new EnrolmentError(400, "Tenant name in use");
    }
}