using System;
using System.Collections.Generic;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.Pipelines.Responses;

namespace Anderson.PackageAudit.Domain
{
    public class Key : IEquatable<Key>
    {
        public string Name { get; set; }
        public Guid Value { get; set; }

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
                return ((Value != null ? Value.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }

    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<UserSummary> Users { get; set; }
        public List<Project> Projects { get; set; }

        public List<Key> Keys { get; set; }
        public List<Account> Accounts { get; set; }


        public Response<Key, Error> GenerateKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return KeyError.InvalidKeyName;
            }

            if (Keys != null && Keys.Any(x => x.Name == name))
            {
                return TenantError.TenantAlreadyContainsKey;
            }
            var value = Guid.NewGuid();
            var key = new Key { Name = name, Value = value };

            Keys = Keys ?? new List<Key>();
            Keys.Add(key);
            return key;
        }

        public void RecordProjectResult(Project project)
        {
            Projects.Add(project);
        }
    }

    public class Project
    {
        public string Name { get; set;  }
        public string Version { get; set;  }
        public IEnumerable<Package> Packages { get; set;  }

        public Project(string name, string version, IEnumerable<Package> packages)
        {
            Name = name;
            Version = version;
            Packages = packages;
        }
    }

    public class Package
    {
        public string PackageManager { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public VulnerabilitySummary Summary { get; set; }
        public IList<Vulnerability> Vulnerabilities { get; set; }
    }

    public class Vulnerability
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Versions { get; set; }
        public string[] References { get; set; }
    }
    public class VulnerabilitySummary
    {
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }

    }

    public class UserSummary
    {
        public string Username { get; set; }
        public List<Account> Accounts { get; set; }
    }

    public class TenantSummary
    {
        public TenantSummary(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public Guid Id { get; set; }
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