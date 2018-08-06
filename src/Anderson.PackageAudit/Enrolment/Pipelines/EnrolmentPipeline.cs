using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Enrolment.Pipelines
{
    public class EnrolmentPipeline : IRequestHandler<HttpRequest>
    {
        private readonly IRequestHandler<HttpRequest> _pipeline;

        public EnrolmentPipeline(IRequestHandler<HttpRequest> pipeline)
        {
            _pipeline = pipeline;
        }
        public Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            return _pipeline.HandleAsync(request, context, token);
        }
    }
}