using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration.Keys
{
    [TestFixture]
    public class KeyGenerationTest
    {
        [Test]
        public void GivenKnownUser_WhenGeneratingKeyForTenant_ThenGeneratesKeyForTenant()
        {
            Assert.Fail();
        }

        [Test]
        public void GivenUnknownUser_WhenGeneratingKey_ThenReturns401()
        {
            Assert.Fail();
        }

        [Test]
        public void GivenDuplicateKeyNameForTenant_WhenGereatingKey_ThenReturns400()
        {
            Assert.Fail();
        }

        [Test]
        public void GivenInvalidKeyName_WhenCreatingKeyForTenant_ThenReturns400()
        {
            Assert.Fail();
        }
        
    }
}
