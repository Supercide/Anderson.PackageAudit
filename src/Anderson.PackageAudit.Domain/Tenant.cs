using System;
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
        public IList<Key> Keys { get; set; }

        public Key GenerateKey(string name)
        {
            var key = new Key(name, Guid.NewGuid());
            Keys.Add(key);
            return key;
        }
    }
}