using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.Infrastructure.Authorization;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.Shared.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Projects.Pipes
{
    public class GetProjectsPipe : PipelineDefinition<ProjectsRequest>
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
            if (IsAuthorizedForTenant(request, context))
            {
                IEnumerable<ProjectResponse> projects = new[]
                {
                    new ProjectResponse
                    {
                        Packages = 10,
                        Vulnerabilities = new Shared.Models.VulnerabilitySummary
                        {
                            High = 10,
                            Low = 9,
                            Unknown = 2,
                            Medium = 6
                        },
                        Version = "3.0.1",
                        LastUpdated = DateTime.Today,
                        Title = "Argon"
                    },
                };

                context.SetResponse(projects);

                return Task.CompletedTask;
            }

            context.SetError(AuthorizationErrors.Unauthorized);
            return Task.CompletedTask;
        }

        private bool IsAuthorizedForTenant(ProjectsRequest request, Context context)
        {
            var account = context[WellKnownContextKeys.Account] as Account;

            var isAuthorizedForTenant = _tenantCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            return isAuthorizedForTenant;
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
                Vulnerabilities = new Shared.Models.VulnerabilitySummary
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