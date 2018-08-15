using System;
using System.Collections.Generic;
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
   
    public class GetPackageVulnerabilitiesPipe : PipelineDefinition<PackageVulnerabilitiesRequest>
    {
        private readonly IMongoCollection<Vulnerability> _vulnerabilitiesCollection;
        private readonly IMongoCollection<Tenant> _tenantsCollection;

        public GetPackageVulnerabilitiesPipe(IMongoCollection<Vulnerability> vulnerabilitiesCollection, IMongoCollection<Tenant> tenantsCollection)
        {
            _vulnerabilitiesCollection = vulnerabilitiesCollection;
            _tenantsCollection = tenantsCollection;
        }

        public override Task HandleAsync(PackageVulnerabilitiesRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            if (IsAuthorizedForTenant(request, context))
            {
                IEnumerable<VulnerabilityResponse> vulnerabilities = new[]
                {
                    new VulnerabilityResponse
                    {
                        Version = "3.0.1",
                        Published = DateTime.Today,
                        Package = "FluentValidation",
                        Title = "ReDos",
                        Project = "Argon V3.2.1",
                        Level = Classification.High
                    },
                };

                context.SetResponse(vulnerabilities);

                return Task.CompletedTask;
            }

            context.SetError(AuthorizationErrors.Unauthorized);
            return Task.CompletedTask;
        }

        private bool IsAuthorizedForTenant(PackageVulnerabilitiesRequest request, Context context)
        {
            var account = context[WellKnownContextKeys.Account] as Account;

            var isAuthorizedForTenant = _tenantsCollection.Find(x => x.Accounts.Contains(account) && x.Name == request.Tenant)
                .Any();

            return isAuthorizedForTenant;
        }
    }
}