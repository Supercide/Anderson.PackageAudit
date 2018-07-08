using System;
using System.Collections.Generic;

namespace Anderson.PackageAudit.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public IList<Account> Accounts { get; set; }

        public IList<TenantSummary> Tenants { get; set; }

        public bool MarketingPreference { get; set; }
        public string Username { get; set; }
    }
}