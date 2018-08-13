﻿using System.Web.Http;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Infrastructure.Errors
{

    public class ErrorResolver : IErrorResolver
    {
        public IActionResult Resolve(Error error)
        {
            switch (error.ErrorType)
            {
                case ErrorType.RequestError:
                    return new BadRequestObjectResult(error);
                case ErrorType.AuthenticationError:
                    return new UnauthorizedResult();
                default:
                    return new InternalServerErrorResult();
            }
        }
    }
}