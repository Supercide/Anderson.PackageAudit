using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Tests.Handlers;
using Anderson.PackageAudit.Users;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Enrollment
{
    class UserEnrollmentTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost.fiddler:{GlobalSetup.Port}")
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));
        }

        [TestCase("anyTenantName", true)]
        [TestCase("anyTenantName", false)]
        public async Task GivenUserNotEnrolled_WhenEnrollingUser_ThenEnrolsUser(string tenantName, bool optInToMarketing)
        {
            var response = await _client.PostAsJsonAsync("/api/users", new EnrolUserRequest
            {
                TenantName = tenantName,
                OptInToMarketing = optInToMarketing
            });

            var user = await response.Content.ReadAsAsync<User>();
            Assert.That(user.Tenants[0].Name, Is.EqualTo(tenantName)); 
        }

        [Test]
        public async Task GivenUserEnrolled_WhenEnrollingUser_ThenReturnsBadRequest()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));

            await EnrolUser("some tenant", true);
            var response = await _client.PostAsJsonAsync("/api/users", new EnrolUserRequest
            {
                TenantName = "another tenant",
                OptInToMarketing = true
            });

            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GivenTenantNameInUse_WhenEnrollingUser_ThenReturnsBadRequest()
        {
            await EnrolUser("some tenant", true);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));
            
            var response = await _client.PostAsJsonAsync("/api/users", new EnrolUserRequest
            {
                TenantName = "some tenant",
                OptInToMarketing = true
            });

            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GivenTenantNameIsInvalid_WhenEnrollingUser_ThenReturnsBadRequest()
        {
            var response = await _client.PostAsJsonAsync("/api/users", new EnrolUserRequest
            {
                TenantName = null,
                OptInToMarketing = true
            });

            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        private Task EnrolUser(string tenantName, bool optInToMarketing)
        {
            return _client.PostAsJsonAsync("/api/users", new EnrolUserRequest
            {
                TenantName = tenantName,
                OptInToMarketing = optInToMarketing
            });
        }
    }

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
