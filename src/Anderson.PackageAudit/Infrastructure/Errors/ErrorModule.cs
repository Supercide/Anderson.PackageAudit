using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Infrastructure.DependancyInjection;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Infrastructure.Errors
{
    public class ErrorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ErrorResolver>()
                .SingleInstance()
                .As<IErrorResolver>();
        }
    }
}
