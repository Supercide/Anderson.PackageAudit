using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Enrollment
{
    public class PackageAuditTest
    {
        [Test]
        public void GivenKnownKey_WhenAuditingPackages_ThenReturnsResultsForPackages()
        {
            Assert.Fail();
        }

        [Test]
        public void GivenUnknownKey_WhenAuditingPackages_ThenReturns401()
        {
            Assert.Fail();
        }

        [Test]
        public void GivenKnownKeyWIthInvalidPackages_WhenAuditingPackages_ThenReturns400()
        {
            Assert.Fail();
        }
    }
}
