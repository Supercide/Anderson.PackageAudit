using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Audit.Pipelines
{
    public class AuditPackagePipeline : Pipeline<HttpRequest>
    {
        public AuditPackagePipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}