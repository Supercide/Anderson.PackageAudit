using System.Collections.Generic;

namespace Anderson.PackageAudit.Domain
{
    public class Team
    {
        public string Name { get; set; }
        public IEnumerable<Key> Keys { get; set; }
    }
}