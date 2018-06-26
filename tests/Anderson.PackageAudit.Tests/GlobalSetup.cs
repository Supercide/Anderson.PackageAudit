using System;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests
{
    [SetUpFixture]
    public class GlobalSetup
    {
    
        [OneTimeSetUp]
        public void Setup()
        {
            string environment = "Test";
            string issuer = "https://watusi.eu.auth0.com/";
            string domain = "watusi.eu.auth0.com";
            string audience = "https://Watusi.Audit.Api";
            string redisConnection = "localhost:32768";
            string mongodbConnection = "mongodb://localhost/AuditorTests";

            Environment.SetEnvironmentVariable("FUNCTION_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("auth0:issuer", issuer);
            Environment.SetEnvironmentVariable("auth0:domain", domain);
            Environment.SetEnvironmentVariable("auth0:audience", audience);
            Environment.SetEnvironmentVariable("redis:connectionstring", redisConnection);
            Environment.SetEnvironmentVariable("mongodb:connectionstring", mongodbConnection);
        }
    }
}