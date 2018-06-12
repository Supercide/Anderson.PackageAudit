using System;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Errors
{
    public static class ErrorExtensions
    {
        public static IActionResult ToActionResult(this Error error, Func<Error, IActionResult> resolver)
        {
            return resolver(error);
        }
    }
}