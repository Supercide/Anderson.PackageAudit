using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public class PipelineModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPackagePipelines, PackagePipelines>();
            serviceCollection.AddScoped<IUserPipelines, UserPipelines>();
            serviceCollection.AddScoped< ITenantPipelines, TenantPipelines > ();
        }
    }
}