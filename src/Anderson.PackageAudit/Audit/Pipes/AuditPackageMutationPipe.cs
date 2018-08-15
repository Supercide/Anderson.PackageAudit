using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Audit.Models;
using Anderson.PackageAudit.SharedPipes.Mutations;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class AuditPackageMutationPipe : HttpRequestMutationPipe<IList<AuditPackageRequest>>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            var model = SerialiseToModel(request);
            return InnerHandler.HandleAsync(model, context, token);
        }
    }
}