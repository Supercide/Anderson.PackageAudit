using System.Collections.Generic;
using Anderson.PackageAudit.Audit.Models;
using Anderson.PackageAudit.Audit.Pipelines;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.Keys.Pipelines;
using Anderson.PackageAudit.Keys.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Autofac;
using Microsoft.AspNetCore.Http;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Audit
{
    public class AuditModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AuditPackagePipe>().SingleInstance().AsSelf();
            containerBuilder.RegisterType<AuditPackageMutationPipe>().SingleInstance().AsSelf();

            containerBuilder.Register(provider =>
            {
                var builder = provider.Resolve<PipelineDefinitionBuilder>();

                return new AuditPackagePipeline(builder.StartWith<HttpRequestPipe, HttpRequest>()
                    .ThenWithMutation<AuditPackageMutationPipe, IList<AuditPackageRequest>>()
                    .ThenWith<AuditPackagePipe>()
                    .Build());
            });                 
        }
    }
}
