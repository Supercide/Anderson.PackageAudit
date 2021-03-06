﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Integration
{
    public class DirectoryHelper
    {
        public static string GetRootDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var rootDirectory = baseDirectory.Substring(0, baseDirectory.IndexOf("tests"));
            return rootDirectory;
        }
    }

    [SetUpFixture]
    public class GlobalSetup
    {
        private AzureFunctionHost _host;
        private const string Project = "Anderson.PackageAudit";
        public const int Port = 7071;

        [OneTimeSetUp]
        public async Task Setup()
       {
            var rootDirectory = DirectoryHelper.GetRootDirectory();
#if DEBUG
            var functionDirectory = Path.Combine(rootDirectory, $@"src\{Project}\bin\Debug\netstandard2.0");
#else
            var functionDirectory = Path.Combine(rootDirectory, $@"src\{Project}\bin\Release\netstandard2.0");
#endif
            _host = new AzureFunctionHost(functionDirectory, Port);

            await _host.StartAsync(new Dictionary<string, string>
            {
                ["FUNCTION_ENVIRONMENT"] = "Test",
                ["auth0:issuer"] = "https://watusi.eu.auth0.com/",
                ["auth0:domain"] = "watusi.eu.auth0.com",
                ["auth0:audience"] = "https://Watusi.Audit.Api",
                ["redis:connectionstring"] = "localhost:32768",
                ["mongodb:connectionstring"] = "mongodb://localhost/AuditorTests",
                ["ossindex:uri"] = "https://ossindex.net/v2.0/package"
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _host.Dispose();
        }
    }
}