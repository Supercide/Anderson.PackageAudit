using System;
using System.Collections.Generic;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class User
    {
        public Guid Id { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        public IEnumerable<Team>  Teams { get; set; }
    }
}