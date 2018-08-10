using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection
{
    public static class ServiceExtensions
    {
        public static void LoadModule<T>(this IServiceCollection source) where T : ServiceModule, new()
        {
            T module = new T();
            module.Load(source);
        }
    }
}