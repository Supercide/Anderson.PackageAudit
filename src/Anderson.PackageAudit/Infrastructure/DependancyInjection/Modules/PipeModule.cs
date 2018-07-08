using System;
using Anderson.PackageAudit.Audit;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Accounts.Pipes;
using Anderson.PackageAudit.SharedPipes.Authorization;
using Anderson.PackageAudit.SharedPipes.Authorization.Pipes;
using Anderson.PackageAudit.SharedPipes.Caching;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.PackageAudit.Users.Pipes;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules
{
    public class PipeModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                object Resolver(Type type) => provider.GetService(type);
                return (Func<Type, object>) Resolver;
            });

            serviceCollection.AddSingleton(typeof(PipelineDefinitionBuilder<,>));
            serviceCollection.AddSingleton(typeof(AuthorizationPipe<>));
            serviceCollection.AddSingleton(typeof(KeyAuthorizationPipe<>));
            serviceCollection.AddSingleton(typeof(AccountMutationPipe<,>));
            serviceCollection.AddSingleton(typeof(HttpRequestMutationPipe<,>));
            serviceCollection.AddSingleton(typeof(AuditRequestCachingPipe));
            serviceCollection.AddSingleton<OSSIndexPipe>();
            serviceCollection.AddSingleton<RetrieveUserPipe>();
            serviceCollection.AddSingleton<EnrolUserPipe>();
        }
    }
}