using System;
using System.Collections.Generic;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules;
using Anderson.PackageAudit.Users.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Users.Pipes
{
    public class EnrolUserPipe : Pipelines.Definitions.PipelineDefinition<EnrolUserRequest, Response<User, Error>>
    {
        private readonly IMongoCollection<User> _userCollection;

        public EnrolUserPipe(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public override Response<User, Error> Handle(EnrolUserRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.TenantName))
                {
                    return UserError.TenantNameInvalid;
                }

                var account = Thread.CurrentPrincipal.ToAccount();

                User user = new User
                {
                    Accounts = new List<Account> {account},
                    Tenants = new List<Tenant> {new Tenant(request.TenantName)},
                    MarketingPreference = request.OptInToMarketing,
                    Id = Guid.NewGuid()
                };

                _userCollection.InsertOne(user);

                return user;
            }
            catch (Exception e)
            {
                if (e.Message.Contains(WellKnownIndexes.AuthenticationIndex))
                {
                    return UserError.UserAlreadyEnrolled;
                }

                return GenericError.InternalServerError;
            }
        }
    }
}