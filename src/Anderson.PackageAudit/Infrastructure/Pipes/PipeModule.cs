using System;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.Pipes
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

            serviceCollection.AddSingleton(typeof(PipelineDefinitionBuilder));
            serviceCollection.AddSingleton(typeof(AuthorizationPipe<>));
            serviceCollection.AddSingleton(typeof(HttpRequestMutationPipe<,>));
        }
    }
}