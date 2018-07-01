using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;

namespace Anderson.PackageAudit.Audit.Errors
{
    public class AuditError : Error
    {
        public static readonly AuditError OssIndexUnavailable = new AuditError(WellKnownAuditErrors.OSSIndexUnavailable, "Unable to connect to sonatype");

        protected AuditError(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}