using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.Projects.Pipelines;
using Anderson.PackageAudit.Projects.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Autofac;
using Microsoft.AspNetCore.Http;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Projects
{
    public class ProjectsModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GetProjectsPipe>().SingleInstance().AsSelf();
            containerBuilder.RegisterType<ProjectsMutationPipe>().SingleInstance().AsSelf();

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                return new GetProjectsPipeline(builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<ProjectsMutationPipe, ProjectsRequest>()
                    .ThenWith<GetProjectsPipe>()
                    .Build());
            });            
        }
    }
}
