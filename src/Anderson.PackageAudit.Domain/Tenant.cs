using System.Collections.Generic;

namespace Anderson.PackageAudit.Domain
{
    public class Tenant
    {
        public Tenant(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public IEnumerable<Key> Keys { get; set; }
    }
}