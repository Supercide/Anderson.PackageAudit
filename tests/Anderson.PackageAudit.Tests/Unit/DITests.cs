using System;
using System.Collections.Generic;
using System.Text;
using Anderson.PackageAudit.Enrolment.Pipelines;
using Anderson.PackageAudit.Tenants.Pipelines;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Unit
{
    [TestFixture]
    public class DITests
    {
        [Test]
        public void GivenEnrolmentIsRegistered_WhenResolvingService_ThenResolvesService()
        {
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();
            var pipeline = provider.GetService<EnrolmentPipeline>();
            pipeline.Should().NotBe(null);
        }

        [Test]
        public void GivenGetTenantsPipeline_WhenResolvingService_ThenResolvesService()
        {
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();
            var pipeline = provider.GetService<GetTenantsPipeline>();
            pipeline.Should().NotBe(null);
        }
    }
}
