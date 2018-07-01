namespace Anderson.PackageAudit.Errors
{
    public abstract class Error
    {
        
        public string ErrorCode { get; }
        public string ErrorMessage { get; }

        protected Error(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
