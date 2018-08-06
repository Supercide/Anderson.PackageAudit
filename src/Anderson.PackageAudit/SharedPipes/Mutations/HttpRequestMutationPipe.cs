using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Anderson.PackageAudit.SharedPipes.Mutations
{
    public class HttpRequestMutationPipe<TModel> : PipelineMutationDefinition<HttpRequest, TModel>
    {
        public override Task HandleAsync(HttpRequest request, Context context, CancellationToken token)
        {
            var model = SerialiseToModel(request);

            return InnerHandler.HandleAsync(model, context, token);
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