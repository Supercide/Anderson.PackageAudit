using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Projects.Pipes
{
    public class ProjectsMutationPipe : HttpRequestMutationPipe<ProjectsRequest>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            return InnerHandler.HandleAsync(new ProjectsRequest
            {
                Tenant = request.Path.Value.Split('/')[1]
            }, context, token);
        }
    }

    public class AuditMutation : HttpRequestMutationPipe<AuditRequest>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            var model = SerialiseToModel(request);
            model.ApiKey = Guid.Parse(request.Headers["X-API-KEY"].First());
            return InnerHandler.HandleAsync(model, context, token);
        }
    }
}