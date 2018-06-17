using System;
using System.Threading;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class KeyGenerationPipe : Pipelines.Definitions.PipelineDefinition<string, Response<Guid, Error>>
    {
        private readonly IMongoCollection<User> _userCollection;

        public KeyGenerationPipe(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public override Response<Guid, Error> Handle(string request)
        {
            Account currentAccount = Thread.CurrentPrincipal.ToAccount();
            return Guid.NewGuid();
        }
    }
}