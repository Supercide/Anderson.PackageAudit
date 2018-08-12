using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.Keys.Pipelines;
using Anderson.PackageAudit.Keys.Pipes;
using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.Packages.Pipelines;
using Anderson.PackageAudit.Packages.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Autofac;
using Microsoft.AspNetCore.Http;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Keys
{
    public class KeysModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GetKeysPipe>().SingleInstance().AsSelf();
            containerBuilder.RegisterType<KeysMutationPipe>().SingleInstance().AsSelf();

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                return new GetKeysPipeline(builder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<KeysMutationPipe, KeysRequest>()
                    .ThenWith<GetKeysPipe>()
                    .Build());
            });            
        }
    }
}
