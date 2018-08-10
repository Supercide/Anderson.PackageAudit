using Anderson.PackageAudit.Enrolment.Models;
using Anderson.PackageAudit.Enrolment.Pipelines;
using Anderson.PackageAudit.Enrolment.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Handlers;
using Autofac;
using Microsoft.AspNetCore.Http;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Enrolment
{
    public class EnrolmentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EnrolmentPipe>()
                   .SingleInstance()
                   .AsSelf();

            builder.Register(provider =>
            {
                var pipelineBuilder = provider.Resolve<PipelineDefinitionBuilder>();

                IRequestHandler<HttpRequest> pipeline =  pipelineBuilder.StartWith<AuthorizationPipe, HttpRequest>()
                    .ThenWithMutation<HttpRequestMutationPipe<EnrolmentRequest>, EnrolmentRequest>()
                    .ThenWith<EnrolmentPipe>()
                    .Build();

                return new EnrolmentPipeline(pipeline);
            }).SingleInstance().AsSelf();
        }
    }

    
}
