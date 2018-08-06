using Anderson.PackageAudit.Enrolment;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules;
using Anderson.PackageAudit.Infrastructure.Errors;
using Anderson.PackageAudit.Infrastructure.Persistence;
using Anderson.PackageAudit.Infrastructure.Persistence.Mongo;
using Anderson.PackageAudit.Infrastructure.Persistence.Redis;
using Anderson.PackageAudit.Infrastructure.Pipes;
using Anderson.PackageAudit.Projects;
using Anderson.PackageAudit.Tenants;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit
{
    public class Startup : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();

            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectBindingProvider(provider));

            var registry = context.Config.GetService<IExtensionRegistry>();
            var filter = new ScopeCleanupFilter();
            registry.RegisterExtension(typeof(IFunctionInvocationFilter), filter);
            registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);
        }

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.LoadModule<EnrolmentModule>();
            serviceCollection.LoadModule<ProjectsModule>();
            serviceCollection.LoadModule<TenantModule>();
            serviceCollection.LoadModule<ErrorModule>();

            serviceCollection.LoadModule<ConfigurationModule>();
            serviceCollection.LoadModule<TokenParametersModule>();

            serviceCollection.LoadModule<MongoModule>();
            serviceCollection.LoadModule<RedisModule>();
            serviceCollection.LoadModule<PipeModule>();
            
        }
    }
}