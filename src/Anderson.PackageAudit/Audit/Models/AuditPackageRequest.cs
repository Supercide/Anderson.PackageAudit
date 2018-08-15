namespace Anderson.PackageAudit.Audit.Models
{
    public class AuditPackageRequest
    {
        public PackageManager Manager { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}