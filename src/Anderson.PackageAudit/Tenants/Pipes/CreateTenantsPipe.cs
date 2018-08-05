using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.SharedPipes.NoOp;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Tenants.Pipes
{
    public class CreateTenantsPipe : Anderson.Pipelines.Definitions.PipelineDefinition<TenantRequest, Response<Unit, Error>>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public CreateTenantsPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override Response<Unit, Error> Handle(TenantRequest request)
        {
            var account = Thread.CurrentPrincipal.ToAccount();

            IEnumerable<TenantResponse> tenants = _tenantCollection.Find(x => x.Accounts.Contains(account))
                .ToList()
                .Select(MapToProjectResponse);

            return Unit.Instance;
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