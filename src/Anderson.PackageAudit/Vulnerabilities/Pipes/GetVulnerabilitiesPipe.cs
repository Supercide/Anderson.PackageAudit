using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure.Authorization;
using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.PackageAudit.Vulnerabilities.Models;
using Anderson.Pipelines.Definitions;
using MongoDB.Driver;
using Classification = Anderson.PackageAudit.Vulnerabilities.Models.Classification;

namespace Anderson.PackageAudit.Vulnerabilities.Pipes
{
   
    public class GetVulnerabilitiesPipe : PipelineDefinition<VulnerabilitiesRequest>
    {
        private readonly IMongoCollection<Vulnerability> _vulnerabilitiesCollection;
        private readonly IMongoCollection<Tenant> _tenantsCollection;

        public GetVulnerabilitiesPipe(IMongoCollection<Vulnerability> vulnerabilitiesCollection, IMongoCollection<Tenant> tenantsCollection)
        {
            _vulnerabilitiesCollection = vulnerabilitiesCollection;
            _tenantsCollection = tenantsCollection;
        }

        public override Task HandleAsync(VulnerabilitiesRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            if (IsAuthorizedForTenant(request, context))
            {
                var vulnerabilities = _vulnerabilitiesCollection.Find(x => x.Tenant == request.Tenant)
                    .ToList()
                    .Select(MapToResponse);

                context.SetResponse(vulnerabilities);

                return Task.CompletedTask;
            }

            context.SetError(AuthorizationErrors.Unauthorized);
            return Task.CompletedTask;
        }

        private VulnerabilityResponse MapToResponse(Vulnerability arg)
        {
            return new VulnerabilityResponse
            {
                Version = arg.Version,
                Published = arg.Published,
                Package = arg.Package,
                Title = arg.Title,
                Project = arg.Project,
                Level = (Classification) arg.Level
            };
        }

        private bool IsAuthorizedForTenant(VulnerabilitiesRequest request, Context context)
        {
            var account = context[WellKnownContextKeys.Account] as Account;

            var isAuthorizedForTenant = _tenantsCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            return isAuthorizedForTenant;
        }
    }
}