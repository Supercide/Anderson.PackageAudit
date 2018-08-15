using System;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Builders;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.Pipes
{
    public class PipeModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(provider =>
            {
                var scope = provider.Resolve<ILifetimeScope>();
                object Resolver(Type type) => scope.Resolve(type);
                return (Func<Type, object>) Resolver;
            }).InstancePerLifetimeScope().AsSelf();

            containerBuilder.RegisterType<HttpRequestPipe>();
            containerBuilder.RegisterType<PipelineDefinitionBuilder>();
            containerBuilder.RegisterType<AuthorizationPipe>();
            containerBuilder.RegisterGeneric(typeof(HttpRequestMutationPipe<>))
                .As(typeof(HttpRequestMutationPipe<>));
        }
    }
}