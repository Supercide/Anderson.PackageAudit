﻿using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.Pipelines.Responses;

namespace Anderson.PackageAudit.Users.Errors
{
    

    public class UserError : Error
    {
        public static readonly UserError NotFound = new UserError(WellKnownUserErrors.UserNotFound, "Usermust be enrolled");
        public static readonly UserError TenantNameInvalid = new UserError(WellKnownUserErrors.TenantNameTaken, "Tenant name is invalid");
        public static readonly UserError UserAlreadyEnrolled = new UserError(WellKnownUserErrors.UserAlreadyEnrolled, "User has already been enrolled");
        public static readonly UserError UsernameInvalid = new UserError(WellKnownUserErrors.UserAlreadyEnrolled, "User name is in use");

        protected UserError(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }

    public class GenericError: Error
    {
        public static readonly GenericError InternalServerError = new GenericError("UNKNOWNERROR", "An unknown error occured");

        protected GenericError(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}