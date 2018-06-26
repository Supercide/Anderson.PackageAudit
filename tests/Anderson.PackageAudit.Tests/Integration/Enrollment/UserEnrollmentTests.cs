using System.Collections.Generic;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Tests.Handlers;
using Anderson.PackageAudit.Users;
using Anderson.PackageAudit.Users.Errors;
using Anderson.PackageAudit.Users.Functions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Enrollment
{
    class UserEnrollmentTests
    {
        private ApplicationServiceLocator _serviceLocator;

        [SetUp]
        public void Setup()
        {
            _serviceLocator = new ApplicationServiceLocator();

        }

        [TestCase("anyTenantName", true)]
        [TestCase("anyTenantName", false)]
        public void GivenUserNotEnrolled_WhenEnrollingUser_ThenEnrolsUser(string tenantName, bool optInToMarketing)
        {
            OkObjectResult response = UserFunctions.Enrol(
                DefaultHttpRequestFactory.CreateWithBody(new EnrolUserRequest
                {
                    TenantName = tenantName,
                    OptInToMarketing = optInToMarketing
                }, new KeyValuePair<string, StringValues>("Authorization", $"Bearer {WellKnownTestTokens.ValidToken}")),
                _serviceLocator.GetScopedService<IErrorResolver<UserError>>(),
                _serviceLocator.GetScopedService<IUserPipelines>()) as OkObjectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Value, Is.TypeOf<User>());
        }

        public void GivenUserEnrolled_WhenEnrollingUser_ThenReturnsBadRequest()
        {
            Assert.Fail();
        }

        public void GivenTenantNameInUser_WhenEnrollingUser_ThenReturnsBadRequest()
        {
            Assert.Fail();
        }

        public void GivenTenantNameIsInvalid_WhenEnrollingUser_ThenReturnsBadRequest()
        {
            Assert.Fail();
        }
    }
}
