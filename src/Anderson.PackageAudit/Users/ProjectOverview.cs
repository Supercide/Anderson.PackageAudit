using System;
using System.Collections.Generic;
using Anderson.PackageAudit.Domain;

namespace Anderson.PackageAudit.Users
{
    public class ProjectOverview
    {
        public string Name { get; set; }
        public Dictionary<Classification, int> VulnerabilitySummary { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Version { get; set; }
        public IEnumerable<string> Managers { get; set; }
    }
}