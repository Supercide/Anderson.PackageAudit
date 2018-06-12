using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Anderson.PackageAudit.SharedPipes.Authorization
{
    public class AuthorizationHandler<TSuccess> : PipelineDefinition<HttpRequest, Response<TSuccess, Error>>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthorizationHandler(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        public override Response<TSuccess, Error> Handle(HttpRequest request)
        {
            try
            {
                var result = ExtractToken(request);
                if (result.IsSuccess)
                {
                    Thread.CurrentPrincipal = ValidateToken(result.Success);
                    return InnerHandler.Handle(request);
                }

                return result.Error;
            }
            catch (Exception e)
            {
                return new Error(WellKnownErrorCodes.UnAuthorized, e.Message);
            }
        }

        private static Response<string, Error> ExtractToken(HttpRequest request)
        {
            if (request.Headers.ContainsKey("Authorization") ||
                request.Headers.ContainsKey("authorization"))
            {
                string value = request.Headers["authorization"].First();
                return value.Substring(7);
            }

            return new Error(WellKnownErrorCodes.UnAuthorized, "request failed authentication");
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.ValidateToken(
                token,
                _tokenValidationParameters,
                out _);
        }
    }
}
