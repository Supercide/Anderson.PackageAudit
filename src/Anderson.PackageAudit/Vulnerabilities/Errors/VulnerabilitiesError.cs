using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Vulnerabilities.Errors
{
    public class VulnerabilitiesError : Error
    {
        public VulnerabilitiesError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}