using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Anderson.PackageAudit.Keys.Pipelines;
using Anderson.PackageAudit.Users;

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
        public StateUnderTestBuilder WithUser(bool optIntoMarketing, string tenantName, string username)
        {
            _userCreation = () => _client.PostAsJsonAsync("/api/users", value: new EnrolUserRequest
            {
                TenantName = tenantName,
                OptInToMarketing = optIntoMarketing,
                Username = username
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
}