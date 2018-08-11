using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.Packages.Pipelines;
using Anderson.PackageAudit.Packages.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.Vulnerabilities.Pipelines;
using Anderson.PackageAudit.Vulnerabilities.Pipes;
using Autofac;
using Microsoft.AspNetCore.Http;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Vulnerabilities
{
    public class VulnerabilitiesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GetVulnerabilitiesPipe>().SingleInstance().AsSelf();
            containerBuilder.RegisterType<VulnerabilityMutationPipe>().SingleInstance().AsSelf();

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                return new GetVulnerabilitiesPipeline(builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<PackageMutationPipe, PackageRequest>()
                    .ThenWith<GetPackagesPipe>()
                    .Build());
            });
        }
    }
}
