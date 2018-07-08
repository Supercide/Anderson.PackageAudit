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
            var response = this.InnerHandler.Handle(request);

            if (response.IsSuccess)
            {
                FilterDefinitionBuilder<Tenant> filterBuilder = new FilterDefinitionBuilder<Tenant>();
                var filter = filterBuilder.ElemMatch(x => x.Keys, key => key.Value == request.ApiKey);
                var tenant = _tenantCollection.FindSync(filter).First();

                tenant.RecordProjectResult(new Project(request.Project, request.Version, response.Success.Packages.Select(package => new Domain.Package
                {
                    Name = package.name,
                    Version = package.version,
                    PackageManager = "nuget",
                    Summary = new VulnerabilitySummary
                    {
                        High = 15,
                        Low = 10,
                        Medium = 3
                    },
                    Vulnerabilities = new List<Vulnerability>()
                })));

                _tenantCollection.ReplaceOne(x => x.Id == tenant.Id, tenant);
            }

            return response;
        }
    }
}