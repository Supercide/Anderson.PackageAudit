using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Users
{
    public class RetrieveTenantPipe : Pipelines.Definitions.PipelineDefinition<HttpRequest, Response<TenantOverview, Error>>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public RetrieveTenantPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override Response<TenantOverview, Error> Handle(HttpRequest request)
        {
            string tenantName = request.Query["name"];
            var account = Thread.CurrentPrincipal.ToAccount();

            var asyncCursor = _tenantCollection.FindSync(x => x.Accounts.Contains(account));
            var tenants = asyncCursor.ToList();
            var tenant = tenants.FirstOrDefault(x => x.Name == tenantName);

            if (tenant == null)
            {
                return TenantError.UnknownTenant;
            }
            List<ProjectOverview> projectOverview = new List<ProjectOverview>();
            var projectVulnerabilitySummary = new Dictionary<Classification, int>();
            foreach (var project in tenant.Projects)
            {
                var vulnerabilitySummary = new Dictionary<Classification, int>();
                foreach (var package in project.Packages)
                {
                    foreach (var summary in package.Vulnerabilities.GroupBy(x => x.Classification))
                    {
                        vulnerabilitySummary[summary.Key] = summary.Count();
                        projectVulnerabilitySummary[summary.Key] = projectVulnerabilitySummary.TryGetValue(summary.Key, out var amount) ? amount + summary.Count() : summary.Count();
                    }
                }
                
                projectOverview.Add(new ProjectOverview
                {
                    Name = project.Name, 
                    Version = project.Version,
                    LastUpdated = DateTime.Today,
                    Managers = new []{ "nuget" },
                    VulnerabilitySummary = vulnerabilitySummary
                });
            }

            return new TenantOverview
            {
                Name = tenant.Name,
                VulnerabilitySummary = projectVulnerabilitySummary,
                Projects = projectOverview,
            };
        }
    }
}