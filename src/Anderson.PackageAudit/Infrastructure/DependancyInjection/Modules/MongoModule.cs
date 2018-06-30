using Anderson.PackageAudit.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public class MongoModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                MongoUrl url = new MongoUrl(configuration["mongodb:connectionstring"]);
                MongoClient client = new MongoClient(url);
                return client.GetDatabase(url.DatabaseName);
            });

            serviceCollection.AddSingleton(provider =>
            {
                var mongodb = provider.GetService<IMongoDatabase>();
                var collection = mongodb.GetCollection<User>(nameof(User));
                var indexDef = new BsonDocument { { "Accounts.Provider", 1 }, { "Accounts.AuthenticationId", 1 } };

                collection.Indexes.CreateOne(indexDef, new CreateIndexOptions
                {
                    Unique = true,
                    Name = WellKnownIndexes.AuthenticationIndex,
                    Sparse = false
                });
                
                return collection;
            });
        }
    }

    public class WellKnownIndexes
    {
        public const string AuthenticationIndex = "AuthenticationIndex";
    }
}