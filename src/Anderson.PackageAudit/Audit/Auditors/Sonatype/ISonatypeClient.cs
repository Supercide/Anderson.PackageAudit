using System.Threading.Tasks;

namespace Anderson.PackageAudit.Audit.Auditors.Sonatype
{
    public interface ISonatypeClient
    {
        Task<Report[]> GetReportAsync(string[] coordinates);
    }
}