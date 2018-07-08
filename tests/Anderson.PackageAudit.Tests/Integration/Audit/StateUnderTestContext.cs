using System;
using System.Collections.Generic;

namespace Anderson.PackageAudit.Tests.Integration.Audit
{
    public class StateUnderTestContext
    {
        public bool UserEnrolled { get; set; }
        public KeyValuePair<string, Guid>[] Keys { get; set; }
    }
}