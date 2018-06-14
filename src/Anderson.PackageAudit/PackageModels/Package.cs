using Anderson.PackageAudit.SharedPipes.Caching;
using Newtonsoft.Json;

namespace Anderson.PackageAudit.PackageModels
{
    public class Package : ICachableEntity
    {
        [JsonIgnore]
        public string Id => $"{name}{version}".ToUpper();
        public string pm { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public int vulnerabilitytotal { get; set; }
        public int vulnerabilitymatches { get; set; }
        public Vulnerability[] vulnerabilities { get; set; }
    }
}