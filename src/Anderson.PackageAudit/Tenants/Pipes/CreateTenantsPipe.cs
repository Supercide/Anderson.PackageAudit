using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure;
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

        public override Task HandleAsync(TenantRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            var account = Thread.CurrentPrincipal.ToAccount();

            if (_tenantCollection.Find(x => x.Name == request.Name).Any())
            {
                context.SetError(TenantError.AlreadyExists);
                return Task.CompletedTask;
            }

            return _tenantCollection.InsertOneAsync(new Tenant
            {
                Name = request.Name,
                Accounts = new List<Account>
                {
                    account
                },
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
            }, cancellationToken: token);;
        }
    }
}