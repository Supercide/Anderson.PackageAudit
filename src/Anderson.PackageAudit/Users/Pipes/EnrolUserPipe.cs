using System;
using System.Collections.Generic;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
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
            var account = Thread.CurrentPrincipal.ToAccount();

            User user = new User
            {
                Accounts = new List<Account> {account},
                Teams = new List<Team>(),
                Id = Guid.NewGuid()
            };

            _userCollection.InsertOne(user);

            return user;
        }
    }
}