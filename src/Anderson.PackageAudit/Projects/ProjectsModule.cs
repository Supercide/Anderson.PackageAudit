using System;
using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules;
using Anderson.PackageAudit.Projects.Functions;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.Projects.Pipelines;
using Anderson.PackageAudit.Projects.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Projects
{
    public class ProjectsModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var builder = provider.GetService<PipelineDefinitionBuilder>();

                return builder.StartWith<AuthorizationPipe<IEnumerable<ProjectResponse>>, HttpRequest, Response<IEnumerable<ProjectResponse>, Error>>()
                    .ThenWithMutation<ProjectsMutationPipe, ProjectsRequest>()
                    .ThenWith<GetProjectsPipe>()
                    .Build() as GetProjectsPipeline;
            });

            serviceCollection.AddSingleton(provider =>
            {
                var builder = provider.GetService<PipelineDefinitionBuilder>();

                return builder.StartWith<AuthorizationPipe<IEnumerable<Package>>, HttpRequest, Response<IEnumerable<Package>, Error>>()
                    .ThenWithMutation<HttpRequestMutationPipe<AuditRequest, Response<IEnumerable<Package>, Error>>, AuditRequest>()
                    .ThenWith<AuditProjectPipe>()
                    .Build() as AuditProjectPipeline;
            });
        }
    }

    
}
