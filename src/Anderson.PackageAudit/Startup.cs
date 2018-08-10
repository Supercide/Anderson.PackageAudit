using Anderson.PackageAudit.Enrolment;
using Anderson.PackageAudit.Infrastructure.Authorization;
using Anderson.PackageAudit.Infrastructure.Configuration;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors;
using Anderson.PackageAudit.Infrastructure.Persistence.Mongo;
using Anderson.PackageAudit.Infrastructure.Persistence.Redis;
using Anderson.PackageAudit.Infrastructure.Pipes;
using Anderson.PackageAudit.Projects;
using Anderson.PackageAudit.Tenants;
using Autofac;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Anderson.PackageAudit
{
    public class Startup : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var builder = new ContainerBuilder();
            ConfigureServices(builder);
            var provider = builder.Build();

            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectBindingProvider(provider));

            var registry = context.Config.GetService<IExtensionRegistry>();
            var filter = new ScopeCleanupFilter();
            registry.RegisterExtension(typeof(IFunctionInvocationFilter), filter);
            registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);
        }

        public static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterModule<EnrolmentModule>();
            builder.RegisterModule<ProjectsModule>();
            builder.RegisterModule<TenantModule>();
            builder.RegisterModule<ErrorModule>();
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<TokenParametersModule>();
            builder.RegisterModule<MongoModule>();
            builder.RegisterModule<RedisModule>();
            builder.RegisterModule<PipeModule>();
            
        }
    }
}