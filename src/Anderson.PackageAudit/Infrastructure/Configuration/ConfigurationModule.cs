using System;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.Configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(provider => new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build())
                .SingleInstance()
                .As<IConfiguration>();
        }
    }
}