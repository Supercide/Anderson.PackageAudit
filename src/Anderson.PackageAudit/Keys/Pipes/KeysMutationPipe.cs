using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.Packages.Models;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Keys.Pipes
{
    public class KeysMutationPipe : PipelineMutationDefinition<HttpRequest, KeysRequest>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            return InnerHandler.HandleAsync(new KeysRequest
            {
                Tenant = (string)context["tenant"]
            }, context, token);
        }
    }
}