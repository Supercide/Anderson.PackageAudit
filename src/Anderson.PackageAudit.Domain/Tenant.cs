using System;
using System.Collections.Generic;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.Pipelines.Responses;

namespace Anderson.PackageAudit.Domain
{
    public class Key : IEquatable<Key>
    {
        public Key(string name, Guid value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; protected set; }
        public Guid Value { get; protected set; }

        public bool Equals(Key other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Key) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Value.GetHashCode();
            }
        }
    }

    public class Tenant
    {
        public Tenant(string name)
        {
            Name = name;
            Keys = new HashSet<Key>();
        }

        public string Name { get; set; }
        public ISet<Key> Keys{ get; set; }

        public Response<KeyValuePair<string, Guid>, Error> GenerateKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return KeyError.InvalidKeyName;
            }

            if (Keys.Any(x => x.Name == name))
            {
                return TenantError.TenantAlreadyContainsKey;
            }
            var value = Guid.NewGuid();
            Keys.Add(new Key(name, value));
            return new KeyValuePair<string, Guid>(name, value);
        }
    }

    public class KeyError : Error
    {
        public KeyError(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }

        public static readonly KeyError InvalidKeyName = new KeyError("InvalidKeyName", "InvalidKeyName");
    }

    public class TenantError : Error
    {
        public static readonly TenantError TenantAlreadyContainsKey = new TenantError("TenantAlreadyContainsKey", "TenantAlreadyContainsKey");
        public static readonly TenantError UnknownTenant = new TenantError("UnknownTenant", "UnknownTenant");

        protected TenantError(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}