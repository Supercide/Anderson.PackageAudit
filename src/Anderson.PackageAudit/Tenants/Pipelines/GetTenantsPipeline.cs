using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Handlers;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Tenants.Pipelines
{
    public class GetTenantsPipeline : Pipeline<HttpRequest>
    {
        public GetTenantsPipeline(IRequestHandler<HttpRequest> pipeline) : base(pipeline)
        {
        }
    }
}