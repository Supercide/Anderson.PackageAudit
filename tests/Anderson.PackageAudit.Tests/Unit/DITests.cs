using Anderson.PackageAudit.Enrolment.Pipelines;
using Anderson.PackageAudit.Tenants.Pipelines;
using Autofac;
using FluentAssertions;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Unit
{
    [TestFixture]
    public class DITests
    {
        [Test]
        public void GivenEnrolmentIsRegistered_WhenResolvingService_ThenResolvesService()
        {
            var builder = new ContainerBuilder();
            Startup.ConfigureServices(builder);
            var provider = builder.Build();
            var pipeline = provider.Resolve<EnrolmentPipeline>();
            pipeline.Should().NotBe(null);
        }

        [Test]
        public void GivenGetTenantsPipeline_WhenResolvingService_ThenResolvesService()
        {
            var builder = new ContainerBuilder();
            Startup.ConfigureServices(builder);
            var provider = builder.Build();
            var pipeline = provider.Resolve<GetTenantsPipeline>();
            pipeline.Should().NotBe(null);
        }
    }
}
