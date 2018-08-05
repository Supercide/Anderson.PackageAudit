using System;
using System.IO;
using System.Linq;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
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