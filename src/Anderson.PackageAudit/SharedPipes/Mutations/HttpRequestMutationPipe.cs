using System.IO;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Anderson.PackageAudit.SharedPipes.Mutations
{
    public class HttpRequestMutationPipe<TModel, TResponse> : PipelineMutationDefinition<HttpRequest, TModel, TResponse>
    {
        public override TResponse Handle(HttpRequest request)
        {
            var model = SerialiseToModel(request);

            return InnerHandler.Handle(model);
        }

        protected static TModel SerialiseToModel(HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.Body))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<TModel>(json);
            }
        }
    }

    
}