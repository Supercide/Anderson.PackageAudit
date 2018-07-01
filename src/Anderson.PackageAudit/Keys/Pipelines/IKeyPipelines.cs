using System;
using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Keys.Pipelines
{
    public interface IKeyPipelines
    {
        IRequestHandler<HttpRequest, Response<KeyValuePair<string, Guid>, Error>> GenerateKey { get; }
    }

    public class KeyPiplines : IKeyPipelines
    {
        private readonly PipelineDefinitionBuilder<HttpRequest, Response<KeyValuePair<string, Guid>, Error>> _builder;

        public KeyPiplines(PipelineDefinitionBuilder<HttpRequest, Response<KeyValuePair<string, Guid>, Error>> builder)
        {
            _builder = builder;
        }

        public IRequestHandler<HttpRequest, Response<KeyValuePair<string, Guid>, Error>> GenerateKey => 
            _builder.StartWith<AuthorizationPipe<KeyValuePair<string, Guid>>>()
                    .ThenWithMutation<HttpRequestMutationPipe<KeyRequest, Response<KeyValuePair<string, Guid>, Error>>, KeyRequest>()
                    .ThenWith<KeyCreationPipe>()
                    .Build();
    }


    public class KeyRequest
    {
        public string Tenant { get; set; }
        public string Name { get; set; }
    }
}