using Anderson.PackageAudit.Core.Errors;

namespace Anderson.PackageAudit.Keys.Errors
{
    public class KeyError : Error
    {
        public KeyError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }

        public static readonly KeyError InvalidKeyName = new KeyError(400, "InvalidKeyName");
        public static readonly KeyError UnknownKey = new KeyError(400, "UnknownKey");
    }
}