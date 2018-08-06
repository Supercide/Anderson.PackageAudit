using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Enrolment.Pipelines
{
    public class EnrolmentPipeline : IRequestHandler<HttpRequest, Response<TenantResponse, Error>>
    {
        private readonly IRequestHandler<HttpRequest, Response<TenantResponse, Error>> _pipeline;

        public EnrolmentPipeline(IRequestHandler<HttpRequest, Response<TenantResponse, Error>> pipeline)
        {
            _pipeline = pipeline;
        }
        public Response<TenantResponse, Error> Handle(HttpRequest request)
        {
            return _pipeline.Handle(request);
        }
    }
}