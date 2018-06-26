using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Keys
{
    [TestFixture]
    public class KeyDeletionTests
    {
        [Test]
        public void GivenExistingKey_WhenDeletingKey_ThenDeletesKey()
        {
            Assert.Fail();
        }

        [Test]
        public void GivenUnknownKey_WhenDeletingKey_ThenReturns404()
        {
            Assert.Fail();
        }

        [Test]
        public void GivenUnknownUser_WhenGeneratingKeyForTenant_ThenReturns401()
        {
            Assert.Fail();
        }
    }
}