using System.Linq;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Users.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Users.Pipes
{
    public class RetrieveUserPipe : Pipelines.Definitions.PipelineDefinition<Account, Response<User, Error>>
    {
        private readonly IMongoCollection<User> _userCollection;

        public RetrieveUserPipe(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public override Response<User, Error> Handle(Account request)
        {
            var user = _userCollection.Find(x => x.Accounts.Contains(request)).FirstOrDefault();

            if (user == null)
            {
                return UserError.NotFound;
            }

            return user;
        }
    }
}