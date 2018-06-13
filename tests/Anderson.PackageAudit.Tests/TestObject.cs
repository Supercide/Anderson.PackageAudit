using Anderson.PackageAudit.SharedPipes.Caching;

namespace Anderson.PackageAudit.Tests
{
    public class TestObject : ICachableEntity
    {
        public string Id { get; set; }
    }
}