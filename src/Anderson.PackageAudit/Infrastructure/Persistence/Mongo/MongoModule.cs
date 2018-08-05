using System.Security.Authentication;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Infrastructure.Persistence
{
    public class MongoModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();

                var mongoUrl = new MongoUrl(configuration["mongodb:connectionstring"]);
                MongoClientSettings settings = MongoClientSettings.FromUrl(mongoUrl);
                settings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };

                var mongoClient = new MongoClient(settings);
                return mongoClient.GetDatabase(mongoUrl.DatabaseName);
                
            });

            serviceCollection.AddSingleton(provider =>
            {
                var mongodb = provider.GetService<IMongoDatabase>();
                var collection = mongodb.GetCollection<User>(nameof(User));
                return collection;
            });

            serviceCollection.AddSingleton(provider =>
            {
                var mongodb = provider.GetService<IMongoDatabase>();
                var collection = mongodb.GetCollection<Tenant>(nameof(Tenant));
                return collection;
            });
        }
    }

    public class WellKnownIndexes
    {
        public const string AuthenticationIndex = "AuthenticationIndex";
    }
}