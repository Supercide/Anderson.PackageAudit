using System;

namespace Anderson.PackageAudit.Tests.Integration.Models
{
    public class TenantResponse
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}