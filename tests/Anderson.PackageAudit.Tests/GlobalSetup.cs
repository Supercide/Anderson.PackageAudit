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
            string issuer = "https://d-magnoxium.eu.auth0.com/";
            string domain = "d-magnoxium.eu.auth0.com";
            string audience = "https://d-magnoxium.com/packagescanner";
            string redisConnection = "localhost:32768";

            Environment.SetEnvironmentVariable("FUNCTION_ENVIRONMENT", environment);
            Environment.SetEnvironmentVariable("auth0:issuer", issuer);
            Environment.SetEnvironmentVariable("auth0:domain", domain);
            Environment.SetEnvironmentVariable("auth0:audience", audience);
            Environment.SetEnvironmentVariable("redis:connectionstring", redisConnection);
        }
    }
}