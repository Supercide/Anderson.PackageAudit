using System;

namespace Anderson.PackageAudit.Projects.Models
{
    public class ProjectResponse
    {
        public string Title { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Version { get; set; }
        public int Packages { get; set; }
        public Vulnerabilities Vulnerabilities { get; set; }
    }
}