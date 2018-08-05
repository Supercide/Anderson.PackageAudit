using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection
{
    public abstract class ServiceModule
    {
        public abstract void Load(IServiceCollection serviceCollection);
    }
}