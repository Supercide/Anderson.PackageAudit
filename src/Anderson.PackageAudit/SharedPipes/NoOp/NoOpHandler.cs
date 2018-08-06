using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;

namespace Anderson.PackageAudit.SharedPipes.NoOp
{
    public class NoOpHandler<TRequest, TResponse> : PipelineDefinition<TRequest>
    {
        private readonly TResponse _response;

        public NoOpHandler(TResponse response)
        {
            _response = response;
        }

        public override Task HandleAsync(TRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}
