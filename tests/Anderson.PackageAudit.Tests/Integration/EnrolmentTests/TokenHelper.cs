using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Anderson.PackageAudit.Tests.Integration.EnrolmentTests
{
    public class TokenHelper
    {
        public static string Token(string sub)
        {
            var jwt = new JwtSecurityToken(
                issuer: "https://watusi.eu.auth0.com/",
                audience: "https://Watusi.Audit.Api",
                claims: new List<Claim>
                {
                    new Claim("sub", sub),
                    new Claim("scope", "openid"),
                    new Claim("azp", "gAkrbAC1BpawB38gYplOZ1Hk1kzWDi01"),
                },
                expires: DateTime.Now.AddMinutes(30));
            jwt.Header["alg"] = "RS256";
            jwt.Header["kid"] = "M0UyQTI0RjY1RTIwNzQ4QTY3QzRBN0NCNjI5NERBRDYyMjQ4OTYxMw";

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}