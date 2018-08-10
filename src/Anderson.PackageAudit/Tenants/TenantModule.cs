using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.PackageAudit.Tenants.Pipelines;
using Anderson.PackageAudit.Tenants.Pipes;
using Anderson.Pipelines.Handlers;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Tenants
{
    public class TenantModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GetTenantsPipe>()
                            .SingleInstance()
                            .AsSelf();

            containerBuilder.RegisterType<CreateTenantsPipe>()
                            .SingleInstance()
                            .AsSelf();

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                IRequestHandler<HttpRequest> pipeline = builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<HttpRequestMutationPipe<Unit>, Unit>()
                    .ThenWith<GetTenantsPipe>()
                    .Build();

                return new GetTenantsPipeline(pipeline);
            });

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                var pipeline = builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<HttpRequestMutationPipe<TenantRequest>, TenantRequest>()
                    .ThenWith<CreateTenantsPipe>()
                    .Build();

                return new CreateTenantPipeline(pipeline);
            });
        }
    }

    
}
