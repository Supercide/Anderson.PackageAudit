using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Keys
{
    [TestFixture]
    public class KeyDeletionTests
    {
        [Test]
        public void GivenExistingKey_WhenDeletingKey_ThenDeletesKey()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void GivenUnknownKey_WhenDeletingKey_ThenReturns404()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void GivenUnknownUser_WhenGeneratingKeyForTenant_ThenReturns401()
        {
            Assert.Inconclusive();
        }
    }
}