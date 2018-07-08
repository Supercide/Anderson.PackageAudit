using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.Tests.Integration.Users;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Audit
{
    public class PackageAuditTest
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{GlobalSetup.Port}"),
                Timeout = TimeSpan.FromDays(1)
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));
        }


        [Test]
        public async Task GivenKnownKey_WhenAuditingPackages_ThenReturnsResultsForPackages()
        {
            StateUnderTestBuilder builder = new StateUnderTestBuilder(_client);

            var context = await builder.WithUser(true, "anyTenant", $"{Guid.NewGuid()}")
                .WithKey("anyKey", "anyTenant")
                .Build();

            _client.DefaultRequestHeaders.Add("X-API-KEY", $"{context.Keys[0].Value}");

            var response = await _client.PostAsJsonAsync("/api/packages", 
                new AuditRequest
                {
                    Packages = new[] { new ProjectPackages { Name = "Fluent.Validation", Version = "1.0.1" } }
                });
            await response.Content.ReadAsAsync<AuditResponse>();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GivenKnownKey_WhenAuditingPackages_ThenAddsProjectToTenant()
        {
            StateUnderTestBuilder builder = new StateUnderTestBuilder(_client);

            var context = await builder.WithUser(true, "anyTenant", $"{Guid.NewGuid()}")
                .WithKey("anyKey", "anyTenant")
                .Build();

            _client.DefaultRequestHeaders.Add("X-API-KEY", $"{context.Keys[0].Value}");

            await _client.PostAsJsonAsync("/api/packages",
                new AuditRequest
                {
                    Project = "anyProject",
                    Version = "anyVersion",
                    Packages = new[] { new ProjectPackages { Name = "Fluent.Validation", Version = "1.0.1" } }
                });

            var tenantJson = await _client.GetStringAsync($"/api/tenants?name=anyTenant");
            var tenant = JsonConvert.DeserializeObject<Tenant>(tenantJson);
            tenant.Projects[0].Name.Should().Be("anyProject");
            tenant.Projects[0].Version.Should().Be("anyVersion");
        }

        public class PackageRequest
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }
        [Test]
        public async Task GivenUnknownKey_WhenAuditingPackages_ThenReturns401()
        {
            _client.DefaultRequestHeaders.Add("X-API-KEY", $"{Guid.NewGuid()}");

            StateUnderTestBuilder builder = new StateUnderTestBuilder(_client);

            var context = await builder.WithUser(true, "anyTenant", $"{Guid.NewGuid()}")
                .Build();

            var response = await _client.PostAsJsonAsync("/api/packages",
                new AuditRequest
                {
                    Packages = new[] { new ProjectPackages { Name = "Fluent.Validation", Version = "1.0.1" } }
                });
            await response.Content.ReadAsAsync<AuditResponse>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void GivenKnownKeyWIthInvalidPackages_WhenAuditingPackages_ThenReturns400()
        {
            Assert.Inconclusive();
        }
    }
}
