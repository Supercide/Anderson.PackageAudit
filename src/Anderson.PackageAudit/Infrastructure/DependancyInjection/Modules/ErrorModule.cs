using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Users.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public class ErrorModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IErrorResolver<AuditError, IActionResult>, AuditErrorResolver>();
            serviceCollection.AddSingleton<IErrorResolver<UserError, IActionResult>, UserErrorResolver>();
        }
    }
}