﻿using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Anderson.PackageAudit.Keys.Pipelines;
using Anderson.PackageAudit.Users.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Anderson.PackageAudit.Keys.Functions
{
    public class KeyFunctions
    {
        [FunctionName("KeyGeneration")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "keys")]HttpRequest req,
            [Inject]IErrorResolver<KeyError> errorResolver,
            [Inject]IKeyPipelines pipelines)
        {
            var pipeline = pipelines.GenerateKey;
            var response = pipeline.Handle(req);
            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Success);
            }

            return response.Error.ToActionResult(errorResolver.Resolve);
        }
    }
}