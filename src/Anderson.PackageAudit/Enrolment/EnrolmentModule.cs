﻿using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Enrolment.Models;
using Anderson.PackageAudit.Enrolment.Pipelines;
using Anderson.PackageAudit.Enrolment.Pipes;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PipelineDefinitionBuilder = Anderson.Pipelines.Builders.PipelineDefinitionBuilder;

namespace Anderson.PackageAudit.Enrolment
{
    public class EnrolmentModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<EnrolmentPipe>();

            serviceCollection.AddSingleton(provider =>
            {
                var builder = provider.GetService<PipelineDefinitionBuilder>();

                IRequestHandler<HttpRequest, Response<TenantResponse, Error>> pipeline =  builder.StartWith<AuthorizationPipe<TenantResponse>, HttpRequest, Response<TenantResponse, Error>>()
                    .ThenWithMutation<HttpRequestMutationPipe<EnrolmentRequest, Response<TenantResponse, Error>>, EnrolmentRequest>()
                    .ThenWith<EnrolmentPipe>()
                    .Build();

                return new EnrolmentPipeline(pipeline);
            });
        }
    }

    
}
