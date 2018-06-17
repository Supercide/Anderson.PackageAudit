using System.Collections.Generic;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class Team
    {
        public string Name { get; set; }
        public IEnumerable<Key> Keys { get; set; }
    }
}