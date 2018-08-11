using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.Infrastructure.Identity;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Tenants.Pipes
{
    public class GetTenantsPipe : Anderson.Pipelines.Definitions.PipelineDefinition<Unit>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public GetTenantsPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override Task HandleAsync(Unit request, Context context, CancellationToken token = default(CancellationToken))
        {
            var account = Thread.CurrentPrincipal.ToAccount();

            IEnumerable<TenantResponse> tenants = _tenantCollection.Find(x => x.Accounts.Contains(account))
                .ToList()
                .Select(MapToProjectResponse);

            context.SetResponse(tenants);

            return Task.CompletedTask;
        }

        private TenantResponse MapToProjectResponse(Tenant tenant)
        {
            return new TenantResponse
            {
                Name = tenant.Name,
                Id = tenant.Id,
                CreatedAt = tenant.CreatedAt,
            };
        }
    }
}