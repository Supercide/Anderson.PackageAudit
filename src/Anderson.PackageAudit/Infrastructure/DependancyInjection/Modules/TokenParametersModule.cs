using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection
{
    public class TokenParametersModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var openIdConfig = provider.GetService<OpenIdConnectConfiguration>();
                return new TokenValidationParameters
                {
                    ValidIssuer = configuration["auth0:issuer"],
                    ValidAudiences = new[] { configuration["auth0:audience"] },
                    IssuerSigningKeys = openIdConfig.SigningKeys,
                    ValidateLifetime = configuration["FUNCTION_ENVIRONMENT"] != WellKnownEnvironments.Test,
                };
            });

            serviceCollection.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var metadataAddress = $"https://{configuration["auth0:domain"]}/.well-known/openid-configuration";

                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    metadataAddress,
                    new OpenIdConnectConfigurationRetriever());

                return configurationManager.GetConfigurationAsync(CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            });
        }
    }
}