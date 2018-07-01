using System;
using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Accounts.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.PackageAudit.Users.Pipes;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Users
{
    public class UserPipelines : IUserPipelines
    {
        private readonly PipelineDefinitionBuilder<HttpRequest, Response<User, Error>> _builder;

        public UserPipelines(PipelineDefinitionBuilder<HttpRequest, Response<User, Error>> builder)
        {
            _builder = builder;
        }

        public IRequestHandler<HttpRequest, Response<User, Error>> RetrieveCurrentUser => _builder.StartWith<AuthorizationPipe<User>>()
            .ThenWithMutation<AccountMutationPipe<HttpRequest, Response<User, Error>>, Account>()
            .ThenWith<RetrieveUserPipe>()
            .Build();

        public IRequestHandler<HttpRequest, Response<User, Error>> EnrolUser => _builder.StartWith<AuthorizationPipe<User>>()
            .ThenWithMutation<HttpRequestMutationPipe<EnrolUserRequest, Response<User, Error>>, EnrolUserRequest>()
            .ThenWith<EnrolUserPipe>()
            .Build();
    }
}
