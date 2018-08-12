using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Keys.Pipelines
{
    public class GetKeysPipeline : Pipeline<HttpRequest>
    {
        public GetKeysPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}