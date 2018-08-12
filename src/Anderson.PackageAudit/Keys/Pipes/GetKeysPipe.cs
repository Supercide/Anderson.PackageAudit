using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anderson.Infrastructure.Authorization;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Definitions;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Keys.Pipes
{
    public class GetKeysPipe : PipelineDefinition<KeysRequest>
    {
        private readonly IMongoCollection<Key> _keyCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public GetKeysPipe(IMongoCollection<Tenant> tenantCollection, IMongoCollection<Key> keyCollection)
        {
            _tenantCollection = tenantCollection;
            _keyCollection = keyCollection;
        }

        public override Task HandleAsync(KeysRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            if (IsAuthorizedForTenant(request, context))
            {
                IEnumerable<KeyResponse> keys = new[]
                {
                    new KeyResponse
                    {
                        Title = "Argon",
                        Value = Guid.NewGuid(),
                        Created = DateTime.UtcNow
                    },
                };

                context.SetResponse(keys);

                return Task.CompletedTask;
            }

            context.SetError(AuthorizationErrors.Unauthorized);
            return Task.CompletedTask;
        }

        private bool IsAuthorizedForTenant(KeysRequest request, Context context)
        {
            var account = context[WellKnownContextKeys.Account] as Account;

            var isAuthorizedForTenant = _tenantCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            return isAuthorizedForTenant;
        }
    }
}