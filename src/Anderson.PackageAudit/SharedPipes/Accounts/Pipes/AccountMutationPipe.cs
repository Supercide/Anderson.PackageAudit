using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Domain;
using Anderson.Pipelines.Definitions;

namespace Anderson.PackageAudit.Audit
{
    public class AccountMutationPipe<TRequest, TResponse> : PipelineMutationDefinition<TRequest, Account, TResponse>
    {
        public override TResponse Handle(TRequest request)
        {
            var account = Thread.CurrentPrincipal.ToAccount();
            return InnerHandler.Handle(account);
        }
    }
}