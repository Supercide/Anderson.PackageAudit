namespace Anderson.PackageAudit.Shared.Models
{
    public class Vulnerabilities
    {
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }
        public int Unknown { get; set; }
    }
}