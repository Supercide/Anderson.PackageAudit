using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Keys.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Keys.Pipelines
{
    public class KeyCreationPipe : Anderson.Pipelines.Definitions.PipelineDefinition<KeyRequest, Response<KeyValuePair<string, Guid>, Error>>
    {
        private readonly IMongoCollection<User> _userCollection;

        public KeyCreationPipe(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public override Response<KeyValuePair<string, Guid>, Error> Handle(KeyRequest request)
        {
            var account = Thread.CurrentPrincipal.ToAccount();
            
            User user = _userCollection.Find(x => x.Accounts.Contains(account))
                                       .FirstOrDefault();

            if (user == null)
            {
                //TODO: this should be bad request
                return AuthorizationErrors.Unauthorized;
            }

            var tentant = user.Tenants.FirstOrDefault(x => x.Name == request.Tenant);

            if (tentant == null)
            {
                return TenantError.UnknownTenant;
            }

            Response<KeyValuePair<string, Guid>, Error> key =  tentant.GenerateKey(request.Name);

            _userCollection.ReplaceOne(x => x.Id == user.Id, user);

            return key;
        }
    }
}