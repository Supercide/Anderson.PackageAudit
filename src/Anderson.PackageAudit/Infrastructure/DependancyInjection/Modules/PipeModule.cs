using System;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization;
using Anderson.PackageAudit.SharedPipes.Caching;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public class PipeModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                object Resolver(Type type) => provider.GetService(type);
                return (Func<Type, object>) Resolver;
            });

            serviceCollection.AddSingleton(typeof(PipelineDefinitionBuilder<,>));
            serviceCollection.AddSingleton(typeof(AuthorizationHandler<>));
            serviceCollection.AddSingleton(typeof(HttpRequestMutationPipe<,>));
            serviceCollection.AddSingleton(typeof(CachingPipe<,>));
            serviceCollection.AddSingleton<OSSIndexPipe>();
        }
    }
}