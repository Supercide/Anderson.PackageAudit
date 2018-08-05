using System;
using System.Collections.Generic;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Projects.Models;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Responses;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Projects.Pipes
{
    public class ProjectsMutationPipe : HttpRequestMutationPipe<ProjectsRequest, Response<IEnumerable<ProjectResponse>, Error>>
    {
        public override Response<IEnumerable<ProjectResponse>, Error> Handle(HttpRequest request)
        {
            return InnerHandler.Handle(new ProjectsRequest
            {
                Tenant = request.Path.Value.Split('/')[1]
            });
        }
    }

    public class AuditMutation : HttpRequestMutationPipe<AuditRequest, Response<AuditResponse, Error>>
    {
        public override Response<AuditResponse, Error> Handle(HttpRequest request)
        {
            var model = SerialiseToModel(request);
            model.ApiKey = Guid.Parse(request.Headers["X-API-KEY"].First());
            return InnerHandler.Handle(model);
        }
    }
}