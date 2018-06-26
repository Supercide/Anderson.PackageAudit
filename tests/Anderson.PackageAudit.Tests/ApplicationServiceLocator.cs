using System;
using Microsoft.Extensions.DependencyInjection;

namespace Anderson.PackageAudit.Tests
{
    public class ApplicationServiceLocator
    {
        public IServiceProvider RootContainer { get; } = CreateServiceProvider();
        public IServiceScope ServiceScope { get; }

        public ApplicationServiceLocator()
        {
            ServiceScope = RootContainer.CreateScope();
        }

        public T GetScopedService<T>()
        {
            return (T)ServiceScope.ServiceProvider.GetService(typeof(T));
        }


        private static ServiceProvider CreateServiceProvider()
        {
            ServiceCollection collection = new ServiceCollection();
            Startup.ConfigureServices(collection);
            var provider = collection.BuildServiceProvider(true);
            return provider;
        }
    }
}