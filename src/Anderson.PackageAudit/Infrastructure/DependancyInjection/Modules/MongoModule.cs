using Anderson.PackageAudit.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                /*IndexKeysDefinitionBuilder<User> builder = new IndexKeysDefinitionBuilder<User>();
                
                 var indexDef = builder(x => x.Accounts[0].AuthenticationId);
                collection.CreateOne().CreateOneAsync(indexDef);*/
                return collection;
            });
        }
    }
}