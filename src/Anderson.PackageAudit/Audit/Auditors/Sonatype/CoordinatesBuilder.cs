namespace Anderson.PackageAudit.Audit.Auditors.Sonatype
{
    public class CoordinatesBuilder : ICoordinatesBuilder
    {
        private PackageType _packageType;
        private string _name;
        private string _version;

        public CoordinatesBuilder WithType(PackageType packageType)
        {
            _packageType = packageType;
            return this;
        }

        public CoordinatesBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CoordinatesBuilder WithVersion(string version)
        {
            _version = version;
            return this;
        }

        public string Build()
        {
            return $"{_packageType}:{_name}@{_version}";
        }
    }
}
