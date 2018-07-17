using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;
using Anderson.Pipelines.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Package = Anderson.PackageAudit.PackageModels.Package;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class OSSIndexPipe : Pipelines.Definitions.PipelineDefinition<AuditRequest, Response<AuditResponse, Error>>
    {
        private readonly IConfiguration _configuration;
        static readonly HttpClient _client = new HttpClient();

        public OSSIndexPipe(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override Response<AuditResponse, Error> Handle(AuditRequest request)
        {
            return HandleAsync(request).GetAwaiter().GetResult();
        }

        public async Task<Response<AuditResponse, Error>> HandleAsync(AuditRequest request)
        {
            if(!request.Packages.Any())
                return new Response<AuditResponse, Error>(new AuditResponse
                {
                    Packages = new PackageSummary[0]
                });

            var requestUri = new Uri(_configuration["ossindex:uri"]);

            using (var response = await _client.PostAsJsonAsync(requestUri, request.Packages.Select(x => new { pm = "nuget", name = x.Name, version = x.Version })))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return new AuditResponse
                    {
                        Packages = Map(json)
                    };
                }

                return AuditError.OssIndexUnavailable;
            }
        }

        public PackageSummary[] Map(string json)
        {
            return JsonConvert.DeserializeObject<Package[]>(json)
                .Select(x => new PackageSummary
                {
                    Name = x.name,
                    Version = x.version,
                    PackageManager = "nuget",
                    Vulnerabilities = x.vulnerabilities?.Select(vulnerability => new VulnerabilitySummary
                    {
                        Versions = vulnerability.versions,
                        Description = vulnerability.description,
                        Classification = Classification.Unknown,
                        Title = vulnerability.title,
                        References = vulnerability.references
                    }).ToArray()
                }).ToArray();
        }
    }
}
