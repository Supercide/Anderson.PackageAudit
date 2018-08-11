using System;
using Anderson.PackageAudit.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Anderson.PackageAudit.Vulnerabilities.Models
{
    public class VulnerabilityResponse
    {
        public string Title { get; set; }
        public Classification Level { get; set; }
        public string Project { get; set; }
        public DateTime Published { get; set; }
        public string Version { get; set; }
        public string Package { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Classification
    {
        Unknown,
        Low,
        Medium,
        High
    }
}