using Anderson.Pipelines.Definitions;

namespace Anderson.PackageAudit.NoOp
{
    public class NoOpHandler<TRequest, TResponse> : PipelineDefinition<TRequest, TResponse>
    {
        private readonly TResponse _response;

        public NoOpHandler(TResponse response)
        {
            _response = response;
        }

        public override TResponse Handle(TRequest request)
        {
            return _response;
        }
    }
}
