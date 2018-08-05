using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace Anderson.PackageAudit.Tests.Unit
{
    public class RetrieveTenantPipeTest
    {
        public static IEnumerable<Tenant> DefaultTenant()
        {
            yield return new Tenant
            {
                Name = "anyTenantName"
            };

            yield return new Tenant
            {
                Name = "anyTenantName",
                Projects = new List<Project>
                {
                    new Project
                    {
                        Title = "any package"
                    }
                }
            };

            yield return new Tenant
            {
                Name = "anyTenantName",
                Projects = new List<Project>
                {
                    new Project
                    {
                        Title = "any project",
                        Packages = new []
                        {
                            new Package
                            {
                                Name = "any package"
                            }, 
                        }
                    }
                }
            };
        } 

        [TestCaseSource(typeof(RetrieveTenantPipeTest), nameof(DefaultTenant))]
        public void GivenPipe_WhenHandlingValidRequest_DoesNotThrowAnException(Tenant tenant)
        {
            int iteration = 0;

            var mockCursor = CreateMockCursor(tenant, () => iteration++, () => iteration <= 1);
            var mockCollection = CreateMockCollection(mockCursor);

            RetrieveTenantPipe pipe = new RetrieveTenantPipe(mockCollection.Object);
            var defaultHttpRequest = CreateDefaultHttpRequest(tenant.Name);
            SetCurrentThreadPrincipal();

            Assert.DoesNotThrow(() => pipe.Handle(defaultHttpRequest));
        }

        private static void SetCurrentThreadPrincipal()
        {
            var principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(new Claim[]
            {
                new Claim(WellKnownOpenIdConnectClaimTypes.Issuer, "anyValue"),
                new Claim(WellKnownOpenIdConnectClaimTypes.NameIdentifier, "anyValue"),
            }));
            Thread.CurrentPrincipal = principal;
        }

        private static DefaultHttpRequest CreateDefaultHttpRequest(string tenantName)
        {
            var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext()
            {
                Request =
                {
                    Query = new QueryCollection(new Dictionary<string, StringValues>
                    {
                        ["name"] = tenantName
                    })
                }
            });
            return defaultHttpRequest;
        }

        private static Mock<IMongoCollection<Tenant>> CreateMockCollection(Mock<IAsyncCursor<Tenant>> mockCursor)
        {
            Mock<IMongoCollection<Tenant>> mockCollection = new Mock<IMongoCollection<Tenant>>();

            mockCollection.Setup(x => x.FindSync<Tenant>(
                    It.IsAny<FilterDefinition<Tenant>>(),
                    It.IsAny<FindOptions<Tenant, Tenant>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(mockCursor.Object);
            return mockCollection;
        }

        private static Mock<IAsyncCursor<Tenant>> CreateMockCursor(Tenant tenant, Action callback, Func<bool> returns)
        {
            var mockCursor = new Mock<IAsyncCursor<Tenant>>();
            mockCursor
                .Setup(x => x.MoveNext(It.IsAny<CancellationToken>()))
                .Callback(callback)
                .Returns(returns);

            mockCursor.Setup(x => x.Current)
                .Returns(new List<Tenant>(new[] {tenant}));
            return mockCursor;
        }
    }
}
