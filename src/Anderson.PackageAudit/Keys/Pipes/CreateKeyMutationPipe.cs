using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.Packages.Models;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Keys.Pipes
{
    public class CreateKeyMutationPipe : HttpRequestMutationPipe<CreateKeyRequest>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            CreateKeyRequest createKeyRequest = SerialiseToModel(request);
            createKeyRequest.Tenant = (string)context["tenant"];
            return InnerHandler.HandleAsync(createKeyRequest, context, token);
        }
    }
}