using System.Collections.Generic;
using Anderson.PackageAudit.SharedPipes.Caching;

namespace Anderson.PackageAudit.PackageModels
{
    public class AuditResponse
    {
        public IList<Package> Packages { get; set; }
    }

    public class AuditRequest
    {
        public string Version { get; set; }
        public string Project { get; set; }
        public IList<ProjectPackages> Packages  { get; set; }
    }

    public class ProjectPackages : ICachableEntity
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string PackageManager { get; set; }
        public string Id => $"{Name}{Version}".ToUpper();
    }
}