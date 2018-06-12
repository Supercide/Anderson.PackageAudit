using System.IO;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Anderson.PackageAudit.Pipelines
{
    public class HttpRequestMutation<TModel, TResponse> : PipelineMutationDefinition<HttpRequest, TModel, TResponse>
    {
        public override TResponse Handle(HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.Body))
            {
                var json = reader.ReadToEnd();
                var model = JsonConvert.DeserializeObject<TModel>(json);
                return InnerHandler.Handle(model);
            }
        }
    }
}