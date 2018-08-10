using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Anderson.PackageAudit.Infrastructure.Authorization
{
    public class TokenParametersModule : Module
    {
        protected override void Load(ContainerBuilder serviceCollection)
        {
            serviceCollection.Register(provider =>
            {
                var configuration = provider.Resolve<IConfiguration>();
                var openIdConfig = provider.Resolve<OpenIdConnectConfiguration>();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["auth0:issuer"],
                    ValidAudiences = new[] { configuration["auth0:audience"] },
                    IssuerSigningKeys = openIdConfig.SigningKeys,
                    
                };

                if (configuration["FUNCTION_ENVIRONMENT"] == WellKnownEnvironments.Test)
                {
                    tokenValidationParameters.ValidateLifetime = false;
                    tokenValidationParameters.ValidateIssuer = false;
                    tokenValidationParameters.ValidateIssuerSigningKey = false;
                    tokenValidationParameters.ValidateActor = false;
                    tokenValidationParameters.ValidateAudience = false;
                    tokenValidationParameters.SignatureValidator = (token, parameters) => new JwtSecurityToken(token);
                }

                return tokenValidationParameters;
            }).SingleInstance().AsSelf();

            serviceCollection.Register(provider =>
            {
                var configuration = provider.Resolve<IConfiguration>();
                var metadataAddress = $"https://{configuration["auth0:domain"]}/.well-known/openid-configuration";

                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    metadataAddress,
                    new OpenIdConnectConfigurationRetriever());

                return configurationManager.GetConfigurationAsync(CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            }).SingleInstance().AsSelf();
        }
    }
}