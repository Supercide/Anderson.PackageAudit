using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure.Identity;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Definitions;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Tenants.Pipes
{
    public class CreateTenantsPipe : PipelineDefinition<TenantRequest>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public CreateTenantsPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override async Task HandleAsync(TenantRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            var account = Thread.CurrentPrincipal.ToAccount();

            if (_tenantCollection.Find(x => x.Name == request.Name).Any())
            {
                context.SetError(TenantError.AlreadyExists);
                return;
            }

            var tenant = new Tenant
            {
                Name = request.Name,
                Accounts = new List<Account>
                {
                    account
                },
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
            };

            await _tenantCollection.InsertOneAsync(tenant, cancellationToken: token);;

            context.SetResponse(new TenantResponse
            {
                Name = tenant.Name,
                Id = tenant.Id,
                CreatedAt = tenant.CreatedAt
            });
        }
    }
}