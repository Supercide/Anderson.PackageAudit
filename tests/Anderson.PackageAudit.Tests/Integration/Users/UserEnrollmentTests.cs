using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Tests.Integration.Audit;
using Anderson.PackageAudit.Users;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Users
{
    class UserTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{GlobalSetup.Port}")
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task GivenKnownUser_WhenRetrievingUserDetails_ThenReturnsUserDetails()
        {
            StateUnderTestBuilder builder = new StateUnderTestBuilder(_client);
            var context = await builder.WithUser(false, "someName")
                                       .WithKey("some key", "someName")
                                       .Build();

            var response = await _client.GetAsync("/api/users");
            var user = await response.Content.ReadAsAsync<User>();
            using (new AssertionScope())
            {
                user.Tenants.Count.Should().Be(1);
                user.Tenants[0].Name.Should().Be("someName");
                user.Tenants[0].Keys.Count.Should().Be(1);
                user.Tenants[0].Keys.Any(x => x.Name == "some key").Should().BeTrue();
            }
        }

        [Test]
        public async Task GivenUnEnrolledUser_WhenRetrievingUserDetails_ThenReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/users");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
    class UserEnrollmentTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{GlobalSetup.Port}")
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

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test, Ignore("not MVP")]
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

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
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
