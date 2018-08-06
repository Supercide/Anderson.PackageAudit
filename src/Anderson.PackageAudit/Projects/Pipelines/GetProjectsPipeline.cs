using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Projects.Pipelines
{
    public interface GetProjectsPipeline : IRequestHandler<HttpRequest>
    {

    }
}