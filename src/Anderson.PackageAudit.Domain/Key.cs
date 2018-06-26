using System;

namespace Anderson.PackageAudit.Domain
{
    public class Key
    {
        public Key(string name, Guid value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; protected set; }
        public Guid Value { get; protected set; }
    }
}