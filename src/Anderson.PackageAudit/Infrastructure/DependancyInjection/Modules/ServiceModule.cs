using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public abstract class ServiceModule
    {
        public abstract void Load(IServiceCollection serviceCollection);
    }
}