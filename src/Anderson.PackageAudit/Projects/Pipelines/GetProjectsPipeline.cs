using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Projects.Pipelines
{
    public class GetProjectsPipeline : Pipeline<HttpRequest>
    {
        public GetProjectsPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}