using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Keys.Pipelines
{
    public class CreateKeysPipeline : Pipeline<HttpRequest>
    {
        public CreateKeysPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}