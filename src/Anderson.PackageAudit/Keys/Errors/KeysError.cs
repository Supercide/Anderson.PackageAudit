using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Keys.Errors
{
    public class KeysError : Error
    {
        public KeysError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}