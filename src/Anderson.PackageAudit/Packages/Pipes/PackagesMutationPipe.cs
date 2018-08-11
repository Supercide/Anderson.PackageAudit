using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Packages.Models;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Packages.Pipes
{
    public class PackagesMutationPipe : PipelineMutationDefinition<HttpRequest, PackagesRequest>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            return InnerHandler.HandleAsync(new PackagesRequest
            {
                Tenant = (string)context["tenant"]
            }, context, token);
        }
    }
}