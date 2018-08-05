using System.Security.Claims;
using System.Security.Principal;

namespace Anderson.PackageAudit.Core.Errors
{
    public abstract class Error
    {

        public int ErrorCode { get; }
        public string ErrorMessage { get; }
        public ErrorType ErrorType { get; }

        protected Error(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorType = GetErrorType(errorCode);
        }

        private ErrorType GetErrorType(int errorCode)
        {
            return ErrorType.RequestError;
        }
    }

    public enum ErrorType
    {
        RequestError,
        AuthenticationError,
        CriticalError
    }

   
}
