using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Projects.Errors
{
    public class ProjectError : Error
    {
        public ProjectError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}