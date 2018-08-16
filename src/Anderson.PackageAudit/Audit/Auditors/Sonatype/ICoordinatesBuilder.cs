namespace Anderson.PackageAudit.Audit.Auditors.Sonatype
{
    public interface ICoordinatesBuilder
    {
        string Build();
        CoordinatesBuilder WithName(string name);
        CoordinatesBuilder WithType(PackageType packageType);
        CoordinatesBuilder WithVersion(string version);
    }
}