using System;

namespace Anderson.PackageAudit.Packages.Models
{
    public class PackagesResponse
    {
        public string Title { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Version { get; set; }
        public int Packages { get; set; }
        public Shared.Models.VulnerabilitySummary Vulnerabilities { get; set; }
    }
}