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
            serviceCollection.AddSingleton(provider =>
            {
                var builder = provider.GetService<PipelineDefinitionBuilder>();

                return builder.StartWith<AuthorizationPipe<IEnumerable<TenantResponse>>, HttpRequest, Response<IEnumerable<TenantResponse>, Error>>()
                    .ThenWithMutation<HttpRequestMutationPipe<Unit, Response<IEnumerable<TenantResponse>, Error>>, Unit>()
                    .ThenWith<GetTenantsPipe>()
                    .Build() as GetTenantsPipeline;
            });

            serviceCollection.AddSingleton(provider =>
            {
                var builder = provider.GetService<PipelineDefinitionBuilder>();

                return builder.StartWith<AuthorizationPipe<Unit>, HttpRequest, Response<Unit, Error>>()
                    .ThenWithMutation<HttpRequestMutationPipe<TenantRequest, Response<Unit, Error>>, TenantRequest>()
                    .ThenWith<CreateTenantsPipe>()
                    .Build() as CreateTenantPipeline;
            });
        }
    }

    
}
