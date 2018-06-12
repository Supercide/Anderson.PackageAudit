using System;
using Microsoft.Extensions.Configuration;

namespace Anderson.PackageAudit.Factories
{
    public static class ConfigurationFactory
    {
        private static readonly Lazy<IConfiguration> _configuration;

        static ConfigurationFactory()
        {
            _configuration = new Lazy<IConfiguration>(() => 
                new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build());
        }

        public static IConfiguration Instance => _configuration.Value;
    }
}