using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Handlers;
using Microsoft.AspNetCore.Http;


    public class Pipeline<TRequest> : IRequestHandler<TRequest>
    {
        protected readonly IRequestHandler<TRequest> _pipeline;

        public Pipeline(IRequestHandler<TRequest> pipeline)
        {
            _pipeline = pipeline;
        }

        public Task HandleAsync(TRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            return _pipeline.HandleAsync(request, context, token);
        }
    }

