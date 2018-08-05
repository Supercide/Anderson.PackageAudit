using System;
using Anderson.PackageAudit.Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Infrastructure.Errors.Extensions
{
    public static class ErrorExtensions
    {
        public static IActionResult ToActionResult(this Error error, Func<Error, IActionResult> resolver)
        {
            return resolver(error);
        }
    }
}