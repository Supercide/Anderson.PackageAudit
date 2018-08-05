using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;
using Package = Anderson.PackageAudit.Projects.Models.Package;

namespace Anderson.PackageAudit.Projects.Pipes
{
    public class AuditProjectPipe : Anderson.Pipelines.Definitions.PipelineDefinition<AuditRequest, Response<IEnumerable<Package>, Error>>
    {
        public override Response<IEnumerable<Package>, Error> Handle(AuditRequest request)
        {
            throw new System.NotImplementedException();
        }
    }

    public class GetProjectsPipe : Anderson.Pipelines.Definitions.PipelineDefinition<ProjectsRequest, Response<IEnumerable<ProjectResponse>, Error>>
    {
        private readonly IMongoCollection<Project> _projectCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public GetProjectsPipe(IMongoCollection<Project> projectCollection, IMongoCollection<Tenant> tenantCollection)
        {
            _projectCollection = projectCollection;
            _tenantCollection = tenantCollection;
        }

        public override Response<IEnumerable<ProjectResponse>, Error> Handle(ProjectsRequest request)
        {
            var account = Thread.CurrentPrincipal.ToAccount();
            var isAuthorizedForTenant = _tenantCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            if (isAuthorizedForTenant)
            {
                var projects =  _projectCollection.Find(x => x.Tenant == request.Tenant)
                    .ToList()
                    .Select(MapToProjectResponse);

                return new Response<IEnumerable<ProjectResponse>, Error>(projects);
            }

            return AuthorizationErrors.Unauthorized;
        }

        private ProjectResponse MapToProjectResponse(Project project)
        {
            var classifications = project
                .Packages
                .SelectMany(package => package.Vulnerabilities.Select(vulnerability => vulnerability.Classification))
                .ToArray();


            return new ProjectResponse
            {
                Version = project.Version,
                LastUpdated = project.LastUpdated,
                Title = project.Title,
                Packages = project.Packages.Count(),
                Vulnerabilities = new Vulnerabilities
                {
                    High = classifications.Count(x => x == Classification.High),
                    Low = classifications.Count(x => x == Classification.Low),
                    Medium = classifications.Count(x => x == Classification.Medium),
                    Unknown = classifications.Count(x => x == Classification.Unknown)
                }
            };
        }
    }
}