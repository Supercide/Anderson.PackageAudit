using System;
using System.Collections.Generic;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.Pipelines.Responses;
using JetBrains.Annotations;

namespace Anderson.PackageAudit.Domain
{
    public class Key
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Tenant { get; set; }
    }

    public class Tenant
    {
        public Tenant()
        {
            Users = new List<UserSummary>();
            Projects = new List<Project>();
            Accounts = new List<Account>();
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<UserSummary> Users { get; set; }
        public List<Project> Projects { get; set; }
        public List<Key> Keys { get; set; }
        public List<Account> Accounts { get; set; }
        public DateTime CreatedAt { get; set; }


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
            Projects = Projects ?? new List<Project>();
            Projects.Add(project);
        }
    }

    public class Vulnerability
    {
        public string Title { get; set; }
        public string Version { get; set; }
    }

    public class Project
    {
        public Project()
        {
            Packages = new Package[0];
        }

        public string Title { get; set;  }
        public string Version { get; set;  }
        
        public IEnumerable<Package> Packages { get; set;  }
        public DateTime LastUpdated { get; set; }
        public string Tenant { get; set; }
    }

    public class Package 
    {
        public Package()
        {
            Vulnerabilities = new VulnerabilitySummary[0];
        }
        public string Name { get; set; }
        public string Version { get; set; }
        public VulnerabilitySummary[] Vulnerabilities { get; set; }
        public string PackageManager { get; set; }
    }
    
    
    public enum Classification
    {
        Unknown,
        Low,
        Medium,
        High
    }

    public class VulnerabilitySummary
    {
        public Classification Classification { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] AffectedVersions { get; set; }
        public string[] References { get; set; }

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
        public KeyError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }

        public static readonly KeyError InvalidKeyName = new KeyError(400, "InvalidKeyName");
    }

    public class TenantError : Error
    {
        public static readonly TenantError TenantAlreadyContainsKey = new TenantError(400, "TenantAlreadyContainsKey");
        public static readonly TenantError UnknownTenant = new TenantError(404, "UnknownTenant");
        public static readonly TenantError AlreadyExists = new TenantError(400, "AlreadyExists");

        protected TenantError(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}