using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.PackageAudit.Tenants.Pipelines;
using Anderson.PackageAudit.Tenants.Pipes;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Tenants
{
    public class TenantModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<GetTenantsPipe, GetTenantsPipe>();
            serviceCollection.AddSingleton<CreateTenantsPipe, CreateTenantsPipe>();
            serviceCollection.AddSingleton(provider =>
            {
                var builder = provider.GetService<PipelineDefinitionBuilder>();

                IRequestHandler<HttpRequest> pipeline = builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<HttpRequestMutationPipe<Unit>, Unit>()
                    .ThenWith<GetTenantsPipe>()
                    .Build();

                return new GetTenantsPipeline(pipeline);
            });

            serviceCollection.AddSingleton(provider =>
            {
                var builder = provider.GetService<PipelineDefinitionBuilder>();

                var pipeline = builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<HttpRequestMutationPipe<TenantRequest>, TenantRequest>()
                    .ThenWith<CreateTenantsPipe>()
                    .Build();

                return new CreateTenantPipeline(pipeline);
            });
        }
    }

    
}
