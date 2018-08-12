using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Vulnerabilities.Pipelines
{
    public class GetProjectVulnerabilitiesPipeline : Pipeline<HttpRequest>
    {
        public GetProjectVulnerabilitiesPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}