using System;
using System.Collections.Generic;
using System.Text;
using Anderson.PackageAudit.Domain;

namespace Anderson.PackageAudit.Projects.Models
{
    public class Package
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public IEnumerable<Vulnerability> Vulnerabilities { get; set; }
    }

    public class Vulnerability
    {
        public Classification Classification { get; set; }
        public string Subject { get; set; }
        public string PacthedIn { get; set; }
        public string[] References { get; set; }
        public string Location { get; set; }
    }

    public class AuditResponse
    {
        public string Project { get; set; }
        public string Version { get; set; }
        public IEnumerable<Package> Packages { get; set; }

    }
}
