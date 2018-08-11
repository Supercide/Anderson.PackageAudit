using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.Packages.Pipes;
using Anderson.PackageAudit.Projects.Pipelines;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Autofac;
using Microsoft.AspNetCore.Http;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Packages
{
    public class PackagesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GetPackagesPipe>().SingleInstance().AsSelf();
            containerBuilder.RegisterType<PackagesMutationPipe>().SingleInstance().AsSelf();

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                return new GetProjectsPipeline(builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<PackagesMutationPipe, PackagesRequest>()
                    .ThenWith<GetPackagesPipe>()
                    .Build());
            });            
        }
    }
}
