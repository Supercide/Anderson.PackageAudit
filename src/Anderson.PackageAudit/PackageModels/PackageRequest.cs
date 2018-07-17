using System;
using System.Collections.Generic;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.SharedPipes.Caching;

namespace Anderson.PackageAudit.PackageModels
{
    public class AuditResponse
    {
        public PackageSummary[] Packages { get; set; }
    }

    public class PackageSummary
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string PackageManager { get; set; }
        public VulnerabilitySummary[] Vulnerabilities { get; set; }
    }

    public class AuditRequest
    {
        public string Version { get; set; }
        public string Project { get; set; }
        public IList<ProjectPackages> Packages  { get; set; }
        public Guid ApiKey { get; set; }
    }

    public class ProjectPackages
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string PackageManager { get; set; }
    }
}