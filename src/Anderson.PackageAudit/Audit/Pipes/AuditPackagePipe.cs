using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Audit.Auditors.Sonatype;
using Anderson.PackageAudit.Audit.Models;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure.Authorization;
using Anderson.PackageAudit.Keys.Errors;
using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Definitions;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class AuditPackagePipe : PipelineDefinition<IList<AuditPackageRequest>>
    {
        private readonly IMongoCollection<Key> _keyCollection;
        private readonly IMongoCollection<Domain.Vulnerability> _vulnerabilitiesCollection;
        private readonly ICoordinatesBuilder _coordinatesBuilder;
        private readonly ISonatypeClient _client;

        public AuditPackagePipe(
            IMongoCollection<Key> keyCollection, 
            IMongoCollection<Domain.Vulnerability> vulnerabilitiesCollection, 
            ICoordinatesBuilder coordinatesBuilder, 
            ISonatypeClient client)
        {
            _keyCollection = keyCollection;
            _vulnerabilitiesCollection = vulnerabilitiesCollection;
            _coordinatesBuilder = coordinatesBuilder;
            _client = client;
        }

        public override async Task HandleAsync(IList<AuditPackageRequest> request, Context context, CancellationToken token = default(CancellationToken))
        {
            var key = GetKey(context);
            if (key != null)
            {
                var coordinates = request.Select(BuildCoordinates).ToArray();

                var report = await _client.GetReportAsync(coordinates);
             
                IList<Vulnerability> response = report.SelectMany(MapToVulnerability).ToList();

                await StoreVulnerabilities(token, response, key);

                context.SetResponse(response);
            }
            else
            {
                context.SetError(KeyError.UnknownKey);
            }
        }

        private async Task StoreVulnerabilities(CancellationToken token, IList<Vulnerability> response, Key key)
        {
            var data = response.Select(x => MapToVulnerabilityStore(x, key.Tenant));

            await _vulnerabilitiesCollection.InsertManyAsync(data, cancellationToken: token);
        }

        private Domain.Vulnerability MapToVulnerabilityStore(Vulnerability arg, string tenant)
        {
            return new Domain.Vulnerability
            {
                Package = arg.Package,
                Tenant = tenant,
                Version = arg.Version,
                Id = Guid.NewGuid(),
                Title = arg.Name,
                Project = "",
                Published = DateTime.Today,
                Level = (Domain.Classification) arg.Classification
            };
        }

        private IEnumerable<Vulnerability> MapToVulnerability(Report report)
        {
            foreach (var vulnerability in report.Vulnerabilities)
            {
                var coords = report.Coordinates.Split(':','@');
                yield return new Vulnerability
                {
                    Name = coords[1],
                    Package = vulnerability.Title,
                    Version = coords[2],
                    Classification = vulnerability.CvssScore > 5 ? Classification.High : Classification.Low,
                    Info = vulnerability.Description
                };
            }
        }

        private string BuildCoordinates(AuditPackageRequest package)
        {
            return _coordinatesBuilder.WithName(package.Name)
                .WithType(PackageType.nuget)
                .WithVersion(package.Version)
                .Build();
        }

        private Key GetKey(Context context)
        {
            return _keyCollection.Find(x => x.Id == Guid.Parse((string)context[WellKnownContextKeys.ApiKey])).FirstOrDefault();
        }
    }
}