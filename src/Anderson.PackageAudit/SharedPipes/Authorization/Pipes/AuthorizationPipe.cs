using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Anderson.Infrastructure.Authorization;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.Infrastructure.Identity;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Anderson.PackageAudit.SharedPipes.Authorization.Pipes
{
    public class AuthorizationPipe : PipelineDefinition<HttpRequest>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthorizationPipe(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            var result = ExtractToken(request);
            if (result.IsSuccess)
            {
                Thread.CurrentPrincipal = ValidateToken(result.Success);
                context[WellKnownContextKeys.Account] = Thread.CurrentPrincipal.ToAccount();
                return InnerHandler.HandleAsync(request, context, token);
            }

            context.SetError(result.Error);

            return Task.CompletedTask;
        }

        private static Response<string, Error> ExtractToken(HttpRequest request)
        {
            if (request.Headers.ContainsKey("Authorization"))
            {
                string value = request.Headers["Authorization"].First();
                return value.Substring(7);
            }

            return Errors.AuthorizationErrors.Unauthorized;
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
