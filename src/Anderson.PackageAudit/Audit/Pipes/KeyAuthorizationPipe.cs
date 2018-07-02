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
        private readonly IMongoCollection<User> _userCollection;

        public KeyAuthorizationPipe(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public override Response<TSuccess, Error> Handle(HttpRequest request)
        {
            try
            {
                
                if (request.Headers.ContainsKey("X-API-KEY") &&
                    Guid.TryParse(request.Headers["X-API-KEY"].FirstOrDefault(), out var key) &&
                    DoesKeyExist(key))
                {
                    return InnerHandler.Handle(request);
                }

                return SharedPipes.Authorization.Errors.AuthorizationErrors.Unauthorized;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            
        }

        private bool DoesKeyExist(Guid key)
        {
            FilterDefinitionBuilder<User> filterBuilder = new FilterDefinitionBuilder<User>();
            var filter = filterBuilder.ElemMatch(x => x.Tenants, tenant => tenant.Keys.Any(k => k.Value == key));
            var cursor = _userCollection.FindSync(filter).Any();
            return cursor;
        }
    }
}