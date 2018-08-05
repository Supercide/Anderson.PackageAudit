using Anderson.PackageAudit.Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Errors
{
    public interface IErrorResolver
    {
        IActionResult Resolve(Error error);
    }
}