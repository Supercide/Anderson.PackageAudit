using System;

namespace Anderson.PackageAudit.Keys.Models
{
    public class KeyResponse
    {
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public Guid Value { get; set; }
    }
}