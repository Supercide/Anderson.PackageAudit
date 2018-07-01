using System;
using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.Pipelines.Responses;

namespace Anderson.PackageAudit.Domain
{
    public class Tenant
    {
        public Tenant(string name)
        {
            Name = name;
            Keys = new Dictionary<string, Guid>();
        }

        public string Name { get; set; }
        public Dictionary<string, Guid> Keys { get; set; }

        public Response<KeyValuePair<string, Guid>, Error> GenerateKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return KeyError.InvalidKeyName;
            }

            if (Keys.ContainsKey(name))
            {
                return TenantError.TenantAlreadyContainsKey;
            }
            var value = Guid.NewGuid();
            Keys.Add(name, value);
            return new KeyValuePair<string, Guid>(name, value);
        }
    }

    public class KeyError : Anderson.PackageAudit.Core.Errors.Error
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