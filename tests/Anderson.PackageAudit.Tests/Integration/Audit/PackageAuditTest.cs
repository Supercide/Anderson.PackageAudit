using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Audit
{
    public class PackageAuditTest
    {
        [Test]
        public void GivenKnownKey_WhenAuditingPackages_ThenReturnsResultsForPackages()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void GivenUnknownKey_WhenAuditingPackages_ThenReturns401()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void GivenKnownKeyWIthInvalidPackages_WhenAuditingPackages_ThenReturns400()
        {
            Assert.Inconclusive();
        }
    }
}
