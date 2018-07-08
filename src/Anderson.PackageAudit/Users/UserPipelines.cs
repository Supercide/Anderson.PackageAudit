using System;
using System.Linq;
using System.Threading;
using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Accounts.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.PackageAudit.Users.Errors;
using Anderson.PackageAudit.Users.Pipes;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Users
{
    public class TenantPipelines : ITenantPipelines
    {
        private readonly PipelineDefinitionBuilder<HttpRequest, Response<Tenant, Error>> _builder;

        public TenantPipelines(PipelineDefinitionBuilder<HttpRequest, Response<Tenant, Error>> builder)
        {
            _builder = builder;
        }

        public IRequestHandler<HttpRequest, Response<Tenant, Error>> RetrieveTenant => _builder.StartWith<AuthorizationPipe<Tenant>>()
            .ThenWith<RetrieveTenantPipe>()
            .Build();
    }

    public class RetrieveTenantPipe : Pipelines.Definitions.PipelineDefinition<HttpRequest, Response<Tenant, Error>>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public RetrieveTenantPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override Response<Tenant, Error> Handle(HttpRequest request)
        {
            string tenantName = request.Query["name"];
            var account = Thread.CurrentPrincipal.ToAccount();

            var tenants = _tenantCollection.FindSync(x => x.Accounts.Contains(account)).ToList();
            var tenant = tenants.FirstOrDefault(x => x.Name == tenantName);

            
            if (tenant == null)
            {
                return TenantError.UnknownTenant;
            }

            return tenant;
        }
    }

    public interface ITenantPipelines
    {
        IRequestHandler<HttpRequest, Response<Tenant, Error>> RetrieveTenant { get; }
    }

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

        public IRequestHandler<HttpRequest, Response<Tenant, Error>> GetTenant { get; }
    }
}
