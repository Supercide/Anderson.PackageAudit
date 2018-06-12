﻿namespace Anderson.PackageAudit.Errors
{
    public class Error
    {
        public string ErrorCode { get; }
        public string ErrorMessage { get; }

        public Error(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
