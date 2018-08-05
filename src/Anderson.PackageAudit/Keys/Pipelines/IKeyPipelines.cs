using System;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
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
        IRequestHandler<HttpRequest, Response<Key, Error>> GenerateKey { get; }
    }

    public class KeyPiplines : IKeyPipelines
    {
        private readonly PipelineDefinitionBuilder _builder;

        public KeyPiplines(PipelineDefinitionBuilder builder)
        {
            _builder = builder;
        }

        public IRequestHandler<HttpRequest, Response<Key, Error>> GenerateKey => 
            _builder.StartWith<AuthorizationPipe<Key>, HttpRequest, Response<Key, Error>>()
                    .ThenWithMutation<HttpRequestMutationPipe<KeyRequest, Response<Key, Error>>, KeyRequest>()
                    .ThenWith<KeyCreationPipe>()
                    .Build();
    }


    public class KeyRequest
    {
        public string Tenant { get; set; }
        public string Name { get; set; }
    }

    public class KeyResponse
    {
        public string Name { get; set; }
        public Guid Value { get; set; }
    }
}