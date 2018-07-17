using System;
using System.Diagnostics;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class KeyAuthorizationPipe<TSuccess> : Pipelines.Definitions.PipelineDefinition<HttpRequest, Response<TSuccess, Error>>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public KeyAuthorizationPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override Response<TSuccess, Error> Handle(HttpRequest request)
        {
           
                
                if (request.Headers.ContainsKey("X-API-KEY") &&
                    Guid.TryParse(request.Headers["X-API-KEY"].FirstOrDefault(), out var key) &&
                    DoesKeyExist(key))
                {
                    return InnerHandler.Handle(request);
                }

                return SharedPipes.Authorization.Errors.AuthorizationErrors.Unauthorized;
           
            
        }

        private bool DoesKeyExist(Guid keyValue)
        {
            FilterDefinitionBuilder<Tenant> filterBuilder = new FilterDefinitionBuilder<Tenant>();
            var filter = filterBuilder.ElemMatch(x => x.Keys, key => key.Value == keyValue);
            return _tenantCollection.FindSync(filter).Any();
        }
    }
}