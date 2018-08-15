using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.SharedPipes.Authorization.Pipes
{
    public class HttpRequestPipe : PipelineDefinition<HttpRequest>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
           return InnerHandler.HandleAsync(request, context, token);
        }
    }
}