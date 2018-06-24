using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Anderson.PackageAudit.Audit.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.SharedPipes.Authorization.Constants;
using Anderson.Pipelines.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class OSSIndexPipe : Pipelines.Definitions.PipelineDefinition<IList<PackageRequest>, Response<IList<Package>, Error>>
    {
        private readonly IConfiguration _configuration;
        static readonly HttpClient _client = new HttpClient();

        public OSSIndexPipe(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override Response<IList<Package>, Error> Handle(IList<PackageRequest> request)
        {
            return HandleAsync(request).GetAwaiter().GetResult();
        }

        public async Task<Response<IList<Package>, Error>> HandleAsync(IList<PackageRequest> request)
        {
            if(!request.Any())
                return new Response<IList<Package>, Error>(new List<Package>());

            var requestUri = new Uri(_configuration["ossindex:uri"]);

            using (var response = await _client.PostAsJsonAsync(requestUri, request.Select(x => new { pm = "nuget", name = x.Name, version = x.Version })))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    IList<Package> packages = JsonConvert.DeserializeObject<IList<Package>>(json);
                    return new Response<IList<Package>, Error>(packages);
                }

                return AuditError.OssIndexUnavailable;
            }
        }
    }
}
