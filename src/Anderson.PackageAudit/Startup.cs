using Anderson.PackageAudit.Enrolment;
using Anderson.PackageAudit.Infrastructure.Authorization;
using Anderson.PackageAudit.Infrastructure.Configuration;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.Errors;
using Anderson.PackageAudit.Infrastructure.Persistence.Mongo;
using Anderson.PackageAudit.Infrastructure.Persistence.Redis;
using Anderson.PackageAudit.Infrastructure.Pipes;
using Anderson.PackageAudit.Keys;
using Anderson.PackageAudit.Packages;
using Anderson.PackageAudit.Projects;
using Anderson.PackageAudit.Tenants;
using Anderson.PackageAudit.Vulnerabilities;
using Autofac;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using MongoDB.Bson.Serialization.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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

            JsonConvert.DefaultSettings = () =>
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                return settings;
            };
        }

        public static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterModule<EnrolmentModule>();
            builder.RegisterModule<ProjectsModule>();
            builder.RegisterModule<PackagesModule>();
            builder.RegisterModule<VulnerabilitiesModule>();
            builder.RegisterModule<TenantModule>();
            builder.RegisterModule<KeysModule>();
            builder.RegisterModule<ErrorModule>();
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<TokenParametersModule>();
            builder.RegisterModule<MongoModule>();
            builder.RegisterModule<RedisModule>();
            builder.RegisterModule<PipeModule>();
        }
    }
}