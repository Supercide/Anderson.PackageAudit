using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Enrollment
{
    class UserEnrollmentTests
    {

        [TestCase("anyTenantName", true)]
        [TestCase("anyTenantName", false)]
        [TestCase("anyTenantName", null)]
        public void GivenUserNotEnrolled_WhenEnrollingUser_ThenEnrolsUser(string tenantName, bool? optInToMarketing)
        {
            Assert.Fail();
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
