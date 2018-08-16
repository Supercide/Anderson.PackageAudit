using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Anderson.PackageAudit.Audit.Auditors.Sonatype;
using Anderson.PackageAudit.Audit.Models;
using Anderson.PackageAudit.Audit.Pipelines;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
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
            
            containerBuilder.RegisterType<Client>()
                .SingleInstance()
                .As<ISonatypeClient>();

            containerBuilder.RegisterType<CoordinatesBuilder>()
                .As<ICoordinatesBuilder>()
                .SingleInstance();

            containerBuilder.Register(ctx => new HttpClient()
            {
                BaseAddress = new Uri("https://ossindex.sonatype.org"),
                DefaultRequestHeaders = { 
                    Accept = {
                        new MediaTypeWithQualityHeaderValue("application/vnd.ossindex.component-report.v1+json")
                    },
                }
            }).SingleInstance().AsSelf();

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
