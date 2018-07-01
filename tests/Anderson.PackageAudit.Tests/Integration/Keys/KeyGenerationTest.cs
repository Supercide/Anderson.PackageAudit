﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Keys.Pipelines;
using Anderson.PackageAudit.Tests.Integration.Enrollment;
using Anderson.PackageAudit.Users;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Keys
{
    [TestFixture]
    public class KeyGenerationTest
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

        private Task EnrolUser(string tenantName, bool optInToMarketing)
        {
            return _client.PostAsJsonAsync("/api/users", new EnrolUserRequest
            {
                TenantName = tenantName,
                OptInToMarketing = optInToMarketing
            });
        }

        [Test]
        public async Task GivenKnownUser_WhenGeneratingKeyForTenant_ThenGeneratesKeyForTenant()
        {
            await EnrolUser("someName", false);

            var expected = "Some key";

            var response = await _client.PostAsJsonAsync("/api/keys", new KeyRequest
            {
                Name = expected,
                Tenant = "someName"
            });

            var key = await response.Content.ReadAsAsync<KeyValuePair<string, Guid>>();

            Assert.That(key.Key, Is.EqualTo(expected));
            Assert.That(key.Value, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task GivenUnknownUser_WhenGeneratingKey_ThenReturns401()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));

            var response = await _client.PostAsJsonAsync("/api/keys", new KeyRequest
            {
                Name = "Some key",
                Tenant = "someName"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GivenDuplicateKeyNameForTenant_WhenGereatingKey_ThenReturns400()
        {
            await EnrolUser("someName", false);

            await GenerateKey("Some key", "someName");

            var response = await _client.PostAsJsonAsync("/api/keys", new KeyRequest
            {
                Name = "Some key",
                Tenant = "someName"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private async Task<HttpResponseMessage> GenerateKey(string name, string tenant)
        {
            var response = await _client.PostAsJsonAsync("/api/keys", new KeyRequest
            {
                Name = name,
                Tenant = tenant
            });
            return response;
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task GivenInvalidKeyName_WhenCreatingKeyForTenant_ThenReturns400(string keyName)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));

            await EnrolUser("someName", false);

            var response = await _client.PostAsJsonAsync("/api/keys", new KeyRequest
            {
                Name = keyName,
                Tenant = "someName"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        
    }

    
}
