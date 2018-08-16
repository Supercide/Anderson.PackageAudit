using System;
using System.Collections.Generic;
using System.Text;

namespace Anderson.PackageAudit.Audit.Auditors.Sonatype
{
    public class ComponentReportRequest
    {
        public string[] coordinates { get; set; }
    }


    public class Report
    {
        public string Coordinates { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public Vulnerability[] Vulnerabilities { get; set; }
    }

    public class Vulnerability
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float CvssScore { get; set; }
        public string CvssVector { get; set; }
        public string Cwe { get; set; }
        public string Reference { get; set; }
    }

}
