using Microsoft.AspNetCore.Mvc;

namespace Anderson.PackageAudit.Errors
{
    public interface IErrorResolver<T> where T : Error
    {
        IActionResult Resolve(Error error);
    }
}