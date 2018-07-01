using System.Linq;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Keys.Pipelines
{
    public class KeyCreationPipe : Anderson.Pipelines.Definitions.PipelineDefinition<KeyRequest, Response<Key, Error>>
    {
        private readonly IMongoCollection<User> _userCollection;

        public KeyCreationPipe(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public override Response<Key, Error> Handle(KeyRequest request)
        {
            var account = Thread.CurrentPrincipal.ToAccount();
            
            User user = _userCollection.Find(x => x.Accounts.Contains(account))
                .First();

            var tentant = user.Tenants.First(x => x.Name == request.Tenant);

            var key =  tentant.GenerateKey(request.Name);

            _userCollection.ReplaceOne(x => x.Id == user.Id, user);

            return key;
        }
    }
}