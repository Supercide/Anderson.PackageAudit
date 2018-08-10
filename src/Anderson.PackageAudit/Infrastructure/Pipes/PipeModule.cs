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
                object Resolver(Type type) => provider.Resolve(type);
                return (Func<Type, object>) Resolver;
            }).SingleInstance().AsSelf();

            containerBuilder.RegisterType<PipelineDefinitionBuilder>();
            containerBuilder.RegisterType<AuthorizationPipe>();
            containerBuilder.RegisterGeneric(typeof(HttpRequestMutationPipe<>))
                .As(typeof(HttpRequestMutationPipe<>));
        }
    }
}