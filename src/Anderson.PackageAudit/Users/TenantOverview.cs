using System.Collections.Generic;
using Anderson.PackageAudit.Domain;

namespace Anderson.PackageAudit.Users
{
    public class TenantOverview
    {
        public string Name { get; set; }
        public Dictionary<Classification, int> VulnerabilitySummary { get; set; }
        public IEnumerable<ProjectOverview> Projects { get; set; }

    }
}