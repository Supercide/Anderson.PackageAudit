using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure.Authorization;
using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Definitions;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Keys.Pipes
{
    public class CreateKeyPipe : PipelineDefinition<CreateKeyRequest>
    {
        private readonly IMongoCollection<Key> _keyCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public CreateKeyPipe(IMongoCollection<Tenant> tenantCollection, IMongoCollection<Key> keyCollection)
        {
            _tenantCollection = tenantCollection;
            _keyCollection = keyCollection;
        }

        public override Task HandleAsync(CreateKeyRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            if (IsAuthorizedForTenant(request, context))
            {
                var key = new Key
                {
                    Name = request.Name,
                    Value = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Tenant = request.Tenant
                };

                _keyCollection.InsertOne(key, cancellationToken: token);

                context.SetResponse(new KeyResponse
                {
                    Title = key.Name,
                    Value = key.Value,
                    Created = key.CreatedAt
                });

                return Task.CompletedTask;
            }

            context.SetError(AuthorizationErrors.Unauthorized);
            return Task.CompletedTask;
        }

        private bool IsAuthorizedForTenant(CreateKeyRequest request, Context context)
        {
            var account = context[WellKnownContextKeys.Account] as Account;

            var isAuthorizedForTenant = _tenantCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            return isAuthorizedForTenant;
        }
    }
}