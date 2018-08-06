using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Projects.Pipes
{
    public class AuditProjectPipe : PipelineDefinition<AuditRequest>
    {
        public override Task HandleAsync(AuditRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }

    public class GetProjectsPipe : Anderson.Pipelines.Definitions.PipelineDefinition<ProjectsRequest>
    {
        private readonly IMongoCollection<Project> _projectCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public GetProjectsPipe(IMongoCollection<Project> projectCollection, IMongoCollection<Tenant> tenantCollection)
        {
            _projectCollection = projectCollection;
            _tenantCollection = tenantCollection;
        }

        public override Task HandleAsync(ProjectsRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            var account = Thread.CurrentPrincipal.ToAccount();
            var isAuthorizedForTenant = _tenantCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            if (!isAuthorizedForTenant)
            {
                context.SetError(AuthorizationErrors.Unauthorized);
                return Task.CompletedTask;
            }

            var projects =  _projectCollection.Find(x => x.Tenant == request.Tenant)
                    .ToList()
                    .Select(MapToProjectResponse);

            context.SetResponse(projects);

            return Task.CompletedTask;
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