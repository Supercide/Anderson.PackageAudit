using Anderson.PackageAudit.SharedPipes.Caching;

namespace Anderson.PackageAudit.PackageModels
{
    public class PackageRequest : ICachableEntity
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Id => $"{Name}{Version}".ToUpper();
    }
}