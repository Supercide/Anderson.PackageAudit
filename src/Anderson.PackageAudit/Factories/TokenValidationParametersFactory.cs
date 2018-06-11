using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Anderson.PackageAudit.Factories
{
    public class TokenValidationParametersFactory
    {
        private static readonly Lazy<TokenValidationParameters> _tokenValidationParameters = new Lazy<TokenValidationParameters>(CreateTokenValidationParameters);

        public static TokenValidationParameters Instance => _tokenValidationParameters.Value;

        private static TokenValidationParameters CreateTokenValidationParameters()
        {
            var config = CreateOpenIdConnectConfiguration()
                            .GetAwaiter()
                            .GetResult();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = ConfigurationFactory.Instance["auth0:issuer"],
                ValidAudiences = new[] { ConfigurationFactory.Instance["auth0:audience"] },
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = ConfigurationFactory.Instance["FUNCTION_ENVIRONMENT"] != WellKnownEnvironments.Test,
            };
            return tokenValidationParameters;
        }

        private static async Task<OpenIdConnectConfiguration> CreateOpenIdConnectConfiguration()
        {
            var metadataAddress = $"https://{ ConfigurationFactory.Instance["auth0:domain"]}/.well-known/openid-configuration";
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(metadataAddress, new OpenIdConnectConfigurationRetriever());
            return await configurationManager.GetConfigurationAsync(CancellationToken.None);
        }
    }
}