using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Anderson.PackageAudit.Tests.Integration.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.EnrolmentTests
{
    [TestFixture]
    public class EnrolmentTest
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{GlobalSetup.Port}")
            };

            GenerateNewIdentity();
        }

        private void GenerateNewIdentity()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task GivenNewUser_WhenEnroling_ThenCreatesTenant()
        {
            var response = await _client.PostAsJsonAsync("api/enrolment", new EnrolmentRequest
            {
                Name = "Magnoxium",
                OptIntoMarketing = true
            });

            var tenant = await response.Content.ReadAsAsync<TenantResponse>();

            using (new AssertionScope())
            {
                tenant.Name.Should().Be("Magnoxium");
                tenant.Id.Should().NotBeEmpty();
                tenant.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, 1000);
            }
        }

        [Test]
        public async Task GivenNewUser_WhenEnrolingTenantThatAleadyExists_ThenReturnsBadRequestWithError()
        {
            await CreateTenant("someTenant");
            GenerateNewIdentity();
            var response = await _client.PostAsJsonAsync("api/enrolment", new EnrolmentRequest
            {
                Name = "someTenant",
                OptIntoMarketing = true
            });
            
            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(400);
            }
        }

        private async Task<HttpResponseMessage> CreateTenant(string name)
        {
            return await _client.PostAsJsonAsync("api/enrolment", new EnrolmentRequest
            {
                Name = name,
                OptIntoMarketing = true
            });
        }
    }
}