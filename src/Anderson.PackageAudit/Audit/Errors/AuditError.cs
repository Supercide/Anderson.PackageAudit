using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Audit.Errors
{
    public class AuditError : Error
    {
        public AuditError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}