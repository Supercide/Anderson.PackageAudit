using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Enrolment.Pipelines
{
    public class EnrolmentPipeline : Pipeline<HttpRequest>
    {
        public EnrolmentPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {

        }
    }
}
