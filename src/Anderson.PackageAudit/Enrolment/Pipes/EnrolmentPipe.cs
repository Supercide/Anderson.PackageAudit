using System;
using System.Collections.Generic;
using System.Threading;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Enrolment.Errors;
using Anderson.PackageAudit.Enrolment.Models;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Enrolment.Pipes
{
    public class EnrolmentPipe : Anderson.Pipelines.Definitions.PipelineDefinition<EnrolmentRequest, Response<TenantResponse, Error>>
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public EnrolmentPipe(IMongoCollection<User> userCollection, IMongoCollection<Tenant> tenantCollection)
        {
            _userCollection = userCollection;
            _tenantCollection = tenantCollection;
        }

        public override Response<TenantResponse, Error> Handle(EnrolmentRequest request)
        {
            var account = Thread.CurrentPrincipal.ToAccount();
            var userExists = _userCollection.Find(x => x.Accounts.Contains(account)).Any();

            var accounts = new List<Account>
            {
                account
            };
            try
            {
                _tenantCollection.InsertOne(new Tenant
                {
                    Name = request.Name,
                    Accounts = accounts
                });
            }
            catch (Exception e)
            {
                return EnrolmentError.TenantNameInUse;
            }
            

            if (!userExists)
            {
                _userCollection.InsertOne(new User
                {
                    Accounts = accounts,
                    MarketingPreference = request.OptIntoMarketing
                });
            }

            return new TenantResponse
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid()
            };
        }
    }
}
