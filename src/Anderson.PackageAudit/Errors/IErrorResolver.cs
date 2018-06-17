using Anderson.PackageAudit.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Audit.Errors
{
    public interface IErrorResolver<T> where T : Error
    {
        IActionResult Resolve(Error error);
    }
}