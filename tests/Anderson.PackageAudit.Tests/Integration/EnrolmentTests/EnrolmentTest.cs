using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
                BaseAddress = new Uri($"http://localhost.fiddler:{GlobalSetup.Port}")
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenHelper.Token(Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task GivenNewUser_WhenEnroling_ThenCreatesTenant()
        {
            var response = await _client.PostAsJsonAsync("api/enrolment", new EnrolmentRequest
            {
                Name = "Magnoxium",
                OptIntoMarketing = true
            });
            var tenants = await response.Content.ReadAsAsync<TenantResponse>();
            using (new AssertionScope())
            {
                tenants.Name.Should().Be("Magnoxium");
            }
        }
    }
}