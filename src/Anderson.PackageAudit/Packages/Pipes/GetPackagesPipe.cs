using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anderson.Infrastructure.Authorization;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Definitions;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Packages.Pipes
{
    public class GetPackagesPipe : PipelineDefinition<PackagesRequest>
    {
        private readonly IMongoCollection<Package> _packageCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public GetPackagesPipe(IMongoCollection<Tenant> tenantCollection, IMongoCollection<Package> packageCollection)
        {
            _tenantCollection = tenantCollection;
            _packageCollection = packageCollection;
        }

        public override Task HandleAsync(PackagesRequest request, Context context, CancellationToken token = default(CancellationToken))
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

        private bool IsAuthorizedForTenant(PackagesRequest request, Context context)
        {
            var account = context[WellKnownContextKeys.Account] as Account;

            var isAuthorizedForTenant = _tenantCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            return isAuthorizedForTenant;
        }
    }
}