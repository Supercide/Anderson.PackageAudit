using System.Collections.Generic;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.PackageModels;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;
using Vulnerability = Anderson.PackageAudit.Domain.Vulnerability;

namespace Anderson.PackageAudit.Audit.Pipes
{

    public class RecordResultPipe : Pipelines.Definitions.PipelineDefinition<AuditRequest, Response<AuditResponse, Error>>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public RecordResultPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override Response<AuditResponse, Error> Handle(AuditRequest request)
        {
            var response = InnerHandler.Handle(request);

            if (response.IsSuccess)
            {
                FilterDefinitionBuilder<Tenant> filterBuilder = new FilterDefinitionBuilder<Tenant>();
                var filter = filterBuilder.ElemMatch(x => x.Keys, key => key.Value == request.ApiKey);
                var tenant = _tenantCollection.FindSync(filter).First();

                tenant.RecordProjectResult(new Project
                {
                    Name = request.Project,
                    Version = request.Version,
                    Packages = response.Success.Packages.Select(package => new Domain.Package
                    {
                        Name = package.Name,
                        Version = package.Version,
                        PackageManager = "nuget",
                        Vulnerabilities = package.Vulnerabilities?.Select(vulnerability => new Vulnerability
                        {
                            Classification = Classification.Unknown,
                            Description = vulnerability.Description,
                            Title = vulnerability.Title,
                            References = vulnerability.References,
                            AffectedVersions = vulnerability.Versions
                        }).ToArray() ?? new Vulnerability[0]
                    })
                });

                _tenantCollection.ReplaceOne(x => x.Id == tenant.Id, tenant);
            }

            return response;
        }
    }
}