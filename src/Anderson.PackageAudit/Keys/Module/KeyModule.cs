using System;
using System.Collections.Generic;
using System.Text;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection.Modules;
using Anderson.PackageAudit.Keys.Pipelines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Keys.Module
{
    public class KeyModule : ServiceModule
    {
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IKeyPipelines, KeyPiplines>();
            serviceCollection.AddSingleton<KeyCreationPipe>();

        }
    }
}
