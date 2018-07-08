using System;
using System.Collections.Generic;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Users.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Users.Pipes
{
    public class EnrolUserPipe : Pipelines.Definitions.PipelineDefinition<EnrolUserRequest, Response<User, Error>>
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public EnrolUserPipe(
            IMongoCollection<User> userCollection,
            IMongoCollection<Tenant> tenantCollection
            )
        {
            _userCollection = userCollection;
            _tenantCollection = tenantCollection;
        }

        public override Response<User, Error> Handle(EnrolUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TenantName))
            {
                return UserError.TenantNameInvalid;
            }

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return UserError.UsernameInvalid;
            }

            var account = Thread.CurrentPrincipal.ToAccount();

            if (_userCollection.FindSync(x => x.Accounts.Contains(account)).Any())
            {
                return UserError.UserAlreadyEnrolled;
            }

            var tenant = new Tenant {
                Name = request.TenantName,
                Users = new List<UserSummary>(new[] 
                {
                    new UserSummary
                    {
                        Accounts = new List<Account>(new []{ account }),
                        Username = request.Username
                    }
                }),
                Accounts = new List<Account>(new[] { account }),
            };

            _tenantCollection.InsertOne(tenant);

            User user = new User
            {
                Username = request.Username,
                Accounts = new List<Account> { account },
                Tenants = new List<TenantSummary> { new TenantSummary(tenant.Name, tenant.Id) },
                MarketingPreference = request.OptInToMarketing,
                Id = Guid.NewGuid()
            };

            _userCollection.InsertOne(user);

            return user;
        }
    }
}