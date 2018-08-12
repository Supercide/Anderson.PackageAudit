using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Vulnerabilities.Pipelines
{
    public class GetPackageVulnerabilitiesPipeline : Pipeline<HttpRequest>
    {
        public GetPackageVulnerabilitiesPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}