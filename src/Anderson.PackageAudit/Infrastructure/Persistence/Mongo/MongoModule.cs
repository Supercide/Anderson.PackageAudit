using System.Security.Authentication;
using Anderson.PackageAudit.Domain;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Infrastructure.Persistence.Mongo
{
    public class MongoModule : Module
    {
        protected override void Load(ContainerBuilder serviceCollection)
        {
            serviceCollection.Register(provider =>
            {
                var configuration = provider.Resolve<IConfiguration>();

                var mongoUrl = new MongoUrl(configuration["mongodb:connectionstring"]);
                MongoClientSettings settings = MongoClientSettings.FromUrl(mongoUrl);
                settings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };

                var mongoClient = new MongoClient(settings);
                return mongoClient.GetDatabase(mongoUrl.DatabaseName);
                
            }).SingleInstance().AsSelf();

            serviceCollection.Register(provider =>
            {
                var mongodb = provider.Resolve<IMongoDatabase>();
                var collection = mongodb.GetCollection<User>(nameof(User));
                return collection;
            }).SingleInstance().AsSelf();

            serviceCollection.Register(provider =>
            {
                var mongodb = provider.Resolve<IMongoDatabase>();
                var collection = mongodb.GetCollection<Tenant>(nameof(Tenant));
                return collection;
            }).SingleInstance().AsSelf();


            serviceCollection.Register(provider =>
            {
                var mongodb = provider.Resolve<IMongoDatabase>();
                var collection = mongodb.GetCollection<Project>(nameof(Project));
                return collection;
            }).SingleInstance().AsSelf();

            serviceCollection.Register(provider =>
            {
                var mongodb = provider.Resolve<IMongoDatabase>();
                var collection = mongodb.GetCollection<Package>(nameof(Package));
                return collection;
            }).SingleInstance().AsSelf();

            serviceCollection.Register(provider =>
            {
                var mongodb = provider.Resolve<IMongoDatabase>();
                var collection = mongodb.GetCollection<Vulnerability>(nameof(Vulnerability));
                return collection;
            }).SingleInstance().AsSelf();

            serviceCollection.Register(provider =>
            {
                var mongodb = provider.Resolve<IMongoDatabase>();
                var collection = mongodb.GetCollection<Key>(nameof(Key));
                return collection;
            }).SingleInstance().AsSelf();
        }
    }

    public class WellKnownIndexes
    {
        public const string AuthenticationIndex = "AuthenticationIndex";
    }
}