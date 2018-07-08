using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anderson.PackageAudit.Keys.Pipelines;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.Tests.Integration.Users;
using Anderson.PackageAudit.Users;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Audit
{
    public class StateUnderTestBuilder
    {
        private readonly HttpClient _client;
        private Func<Task> _userCreation;
        private readonly List<Func<Task<KeyValuePair<string, Guid>>>> _keyCreation = new List<Func<Task<KeyValuePair<string, Guid>>>>();

        public StateUnderTestBuilder(HttpClient client)
        {
            _client = client;
        }
        public StateUnderTestBuilder WithUser(bool optIntoMarketing, string tenantName)
        {
            _userCreation = () => _client.PostAsJsonAsync("/api/users", new EnrolUserRequest
            {
                TenantName = tenantName,
                OptInToMarketing = optIntoMarketing
            });

            return this;
        }

        public StateUnderTestBuilder WithKey(string name, string tenantName)
        {
            _keyCreation.Add(async () =>
            {
                var response = await _client.PostAsJsonAsync("/api/keys", new KeyRequest
                {
                    Name = name,
                    Tenant = tenantName
                });

                return await response.Content.ReadAsAsync<KeyValuePair<string, Guid>>();
            });

            return this;
        }

        public async Task<StateUnderTestContext> Build()
        {
            _userCreation?.Invoke().GetAwaiter().GetResult();
            var keys = await Task.WhenAll(_keyCreation.Select(createKey => createKey()));
            return new StateUnderTestContext
            {
                UserEnrolled = _userCreation != null,
                Keys = keys
            };
        }
    }

    public class StateUnderTestContext
    {
        public bool UserEnrolled { get; set; }
        public KeyValuePair<string, Guid>[] Keys { get; set; }
    }

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

            var context = await builder.WithUser(true, "anyTenant")
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

            var context = await builder.WithUser(true, "anyTenant")
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
