using System;
using Anderson.PackageAudit.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests
{
    public class AuthorizationTests
    {
        public void SetEnvironmentVariables(
            string environment = "Test", 
            string issuer = "https://d-magnoxium.eu.auth0.com/", 
            string domain = "d-magnoxium.eu.auth0.com", 
            string audience = "https://d-magnoxium.com/packagescanner")
        {
            Environment.SetEnvironmentVariable("FUNCTION_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("auth0:issuer", issuer);
            Environment.SetEnvironmentVariable("auth0:domain", domain);
            Environment.SetEnvironmentVariable("auth0:audience", audience);

            StaticClassHelper.Reset(typeof(ConfigurationFactory));
            StaticClassHelper.Reset(typeof(TokenValidationParametersFactory));
        }
          
        [Test]
        public void GivenTokenIsNotPresent_WhenMakingARequestToFunction_ThenFunctionReturnsUnauthorized()
        {
            SetEnvironmentVariables();

            IActionResult response = PackageAuditor.Run(new DefaultHttpRequest(new DefaultHttpContext()));

            Assert.That(response, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public void GivenExpiredTokenIsPresent_WhenMakingARequestToFunction_ThenFunctionReturnsUnauthorized()
        {
            SetEnvironmentVariables(environment: "unit test");

            var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Headers = { ["Authorization"] = $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik5qazVPVFkzUWpFelFrSTBSRVpDTVRWQlJUVTRPVVpDUWpGR1FUaEROVEl5UXpRek9EbENSZyJ9.eyJpc3MiOiJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw1YjEzMzA1MjdiOTQ0OTMxZjQwMWE4ODkiLCJhdWQiOlsiaHR0cHM6Ly9kLW1hZ25veGl1bS5jb20vcGFja2FnZXNjYW5uZXIiLCJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE1Mjg2MDMyNjAsImV4cCI6MTUyODYxMDQ2MCwiYXpwIjoiSnN4b3hET2sydFpicUZZa3V1NzNpd2I2cmxxNVdzNmkiLCJzY29wZSI6Im9wZW5pZCJ9.GHtZVZ3oIKTFsgzjKOR4XBEcRIbM1re-rGyXHaNMWyGgukcDG55LFa_Mhoo-8QteshEIAAFzGOfdDmiZd5TTkpQBsHazqjhZzzXVSo3dOfA2Jx3i2dE42jFMhpAiKYy2sCxCfY7UqdE5y9tW7P-DcXsEFqgt02Y75unsgtKegx50s6jxtGBBvsyyNTPJR_RfXpMDLwPjEVIUmwvh70C6LWH52lYK6NDp06sbWnjhnCfX1u8XS9tr7znbRqb-EItkz64ziZ_pdTKs8RLTLS5NuBd--zPyQv8GO5kEmDH2Ljytcp6jNqidBkxwRRr8GQI5T0KJqZgzJvAjXnKbhV1mEQ" }
            };

            IActionResult response = PackageAuditor.Run(defaultHttpRequest);

            Assert.That(response, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public void GivenUnknownIssuerOnToken_WhenMakingARequestToFunction_ThenFunctionReturnsUnauthorized()
        {
            SetEnvironmentVariables(issuer: "https://example.com/");

            var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Headers = { ["Authorization"] = $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik5qazVPVFkzUWpFelFrSTBSRVpDTVRWQlJUVTRPVVpDUWpGR1FUaEROVEl5UXpRek9EbENSZyJ9.eyJpc3MiOiJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw1YjEzMzA1MjdiOTQ0OTMxZjQwMWE4ODkiLCJhdWQiOlsiaHR0cHM6Ly9kLW1hZ25veGl1bS5jb20vcGFja2FnZXNjYW5uZXIiLCJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE1Mjg2MDMyNjAsImV4cCI6MTUyODYxMDQ2MCwiYXpwIjoiSnN4b3hET2sydFpicUZZa3V1NzNpd2I2cmxxNVdzNmkiLCJzY29wZSI6Im9wZW5pZCJ9.GHtZVZ3oIKTFsgzjKOR4XBEcRIbM1re-rGyXHaNMWyGgukcDG55LFa_Mhoo-8QteshEIAAFzGOfdDmiZd5TTkpQBsHazqjhZzzXVSo3dOfA2Jx3i2dE42jFMhpAiKYy2sCxCfY7UqdE5y9tW7P-DcXsEFqgt02Y75unsgtKegx50s6jxtGBBvsyyNTPJR_RfXpMDLwPjEVIUmwvh70C6LWH52lYK6NDp06sbWnjhnCfX1u8XS9tr7znbRqb-EItkz64ziZ_pdTKs8RLTLS5NuBd--zPyQv8GO5kEmDH2Ljytcp6jNqidBkxwRRr8GQI5T0KJqZgzJvAjXnKbhV1mEQ" }
            };

            IActionResult response = PackageAuditor.Run(defaultHttpRequest);

            Assert.That(response, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public void GivenUnknownAudienceOnToken_WhenMakingARequestToFunction_ThenFunctionReturnsUnauthorized()
        {
            SetEnvironmentVariables(audience: "https://example.com/");

            var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Headers = { ["Authorization"] = $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik5qazVPVFkzUWpFelFrSTBSRVpDTVRWQlJUVTRPVVpDUWpGR1FUaEROVEl5UXpRek9EbENSZyJ9.eyJpc3MiOiJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw1YjEzMzA1MjdiOTQ0OTMxZjQwMWE4ODkiLCJhdWQiOlsiaHR0cHM6Ly9kLW1hZ25veGl1bS5jb20vcGFja2FnZXNjYW5uZXIiLCJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE1Mjg2MDMyNjAsImV4cCI6MTUyODYxMDQ2MCwiYXpwIjoiSnN4b3hET2sydFpicUZZa3V1NzNpd2I2cmxxNVdzNmkiLCJzY29wZSI6Im9wZW5pZCJ9.GHtZVZ3oIKTFsgzjKOR4XBEcRIbM1re-rGyXHaNMWyGgukcDG55LFa_Mhoo-8QteshEIAAFzGOfdDmiZd5TTkpQBsHazqjhZzzXVSo3dOfA2Jx3i2dE42jFMhpAiKYy2sCxCfY7UqdE5y9tW7P-DcXsEFqgt02Y75unsgtKegx50s6jxtGBBvsyyNTPJR_RfXpMDLwPjEVIUmwvh70C6LWH52lYK6NDp06sbWnjhnCfX1u8XS9tr7znbRqb-EItkz64ziZ_pdTKs8RLTLS5NuBd--zPyQv8GO5kEmDH2Ljytcp6jNqidBkxwRRr8GQI5T0KJqZgzJvAjXnKbhV1mEQ" }
            };

            IActionResult response = PackageAuditor.Run(defaultHttpRequest);

            Assert.That(response, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public void GivenValidToken_WhenMakingARequestToFunction_ThenFunctionReturnsUnauthorized()
        {
            SetEnvironmentVariables();

            var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Headers = { ["Authorization"] = $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik5qazVPVFkzUWpFelFrSTBSRVpDTVRWQlJUVTRPVVpDUWpGR1FUaEROVEl5UXpRek9EbENSZyJ9.eyJpc3MiOiJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw1YjEzMzA1MjdiOTQ0OTMxZjQwMWE4ODkiLCJhdWQiOlsiaHR0cHM6Ly9kLW1hZ25veGl1bS5jb20vcGFja2FnZXNjYW5uZXIiLCJodHRwczovL2QtbWFnbm94aXVtLmV1LmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE1Mjg2MDMyNjAsImV4cCI6MTUyODYxMDQ2MCwiYXpwIjoiSnN4b3hET2sydFpicUZZa3V1NzNpd2I2cmxxNVdzNmkiLCJzY29wZSI6Im9wZW5pZCJ9.GHtZVZ3oIKTFsgzjKOR4XBEcRIbM1re-rGyXHaNMWyGgukcDG55LFa_Mhoo-8QteshEIAAFzGOfdDmiZd5TTkpQBsHazqjhZzzXVSo3dOfA2Jx3i2dE42jFMhpAiKYy2sCxCfY7UqdE5y9tW7P-DcXsEFqgt02Y75unsgtKegx50s6jxtGBBvsyyNTPJR_RfXpMDLwPjEVIUmwvh70C6LWH52lYK6NDp06sbWnjhnCfX1u8XS9tr7znbRqb-EItkz64ziZ_pdTKs8RLTLS5NuBd--zPyQv8GO5kEmDH2Ljytcp6jNqidBkxwRRr8GQI5T0KJqZgzJvAjXnKbhV1mEQ" }
            };

            IActionResult response = PackageAuditor.Run(defaultHttpRequest);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }
    }
}
