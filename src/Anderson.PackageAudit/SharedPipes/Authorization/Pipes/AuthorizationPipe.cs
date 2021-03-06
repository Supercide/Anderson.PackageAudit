﻿using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Anderson.PackageAudit.SharedPipes.Authorization.Pipes
{
    public class AuthorizationPipe<TSuccess> : PipelineDefinition<HttpRequest, Response<TSuccess, Error>>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthorizationPipe(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        public override Response<TSuccess, Error> Handle(HttpRequest request)
        {
            
                var result = ExtractToken(request);
                if (result.IsSuccess)
                {
                    Thread.CurrentPrincipal = ValidateToken(result.Success);
                    return InnerHandler.Handle(request);
                }

                return result.Error;
            
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
