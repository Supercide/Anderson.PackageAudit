using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace Anderson.PackageAudit.Infrastructure.DependancyInjection
{
    public class InjectBindingProvider : IBindingProvider
    {
        public static readonly ConcurrentDictionary<Guid, ILifetimeScope> Scopes =
            new ConcurrentDictionary<Guid, ILifetimeScope>();

        private readonly ILifetimeScope _lifetime;

        public InjectBindingProvider(ILifetimeScope lifetimeScope)
        {
            _lifetime = lifetimeScope;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new InjectBinding(_lifetime, context.Parameter.ParameterType);
            return Task.FromResult(binding);
        }
    }
}