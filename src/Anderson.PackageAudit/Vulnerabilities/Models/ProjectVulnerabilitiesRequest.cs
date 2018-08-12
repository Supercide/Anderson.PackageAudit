namespace Anderson.PackageAudit.Vulnerabilities.Models
{
    public class ProjectVulnerabilitiesRequest
    {
        public string Tenant { get; set; }
        public string Project { get; set; }
    }
}