using System;
using System.Collections.Generic;
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

            if (_tenantCollection.Find(x => x.Name == request.Name).Any())
            {
                return TenantError.AlreadyExists;
            }

            _tenantCollection.InsertOne(new Tenant
            {
                Name = request.Name,
                Accounts = new List<Account>
                {
                    account
                },
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
            });;

            return Unit.Instance;
        }
    }
}