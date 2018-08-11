using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Vulnerabilities.Pipelines
{
    public class GetVulnerabilitiesPipeline : Pipeline<HttpRequest>
    {
        public GetVulnerabilitiesPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}