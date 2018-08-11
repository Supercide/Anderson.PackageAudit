using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Packages.Pipelines
{
    public class GetPackagesPipeline : Pipeline<HttpRequest>
    {
        public GetPackagesPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}