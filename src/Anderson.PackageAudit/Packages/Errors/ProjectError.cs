using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Projects.Errors
{
    public class PackgeError : Error
    {
        public PackgeError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}