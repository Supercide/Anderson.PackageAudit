namespace Anderson.PackageAudit.Vulnerabilities.Models
{
    public class PackageVulnerabilitiesRequest
    {
        public string Tenant { get; set; }
        public string Package { get; set; }
    }
}