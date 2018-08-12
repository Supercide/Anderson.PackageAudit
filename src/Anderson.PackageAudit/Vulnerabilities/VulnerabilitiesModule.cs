using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.Packages.Pipelines;
using Anderson.PackageAudit.Packages.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.Vulnerabilities.Models;
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
            containerBuilder.RegisterType<PackageVulnerabilityMutationPipe>().SingleInstance().AsSelf();
            containerBuilder.RegisterType<GetPackageVulnerabilitiesPipe>().SingleInstance().AsSelf();

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                return new GetVulnerabilitiesPipeline(builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<VulnerabilityMutationPipe, VulnerabilitiesRequest>()
                    .ThenWith<GetVulnerabilitiesPipe>()
                    .Build());
            });

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                return new GetPackageVulnerabilitiesPipeline(builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<PackageVulnerabilityMutationPipe, PackageVulnerabilitiesRequest>()
                    .ThenWith<GetPackageVulnerabilitiesPipe>()
                    .Build());
            });
        }
    }
}
