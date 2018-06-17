using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Users.Errors;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public class ErrorModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IErrorResolver<AuditError>, AuditErrorResolver>();
            serviceCollection.AddSingleton<IErrorResolver<UserError>, UserErrorResolver>();
        }
    }
}