using System.Collections.Generic;
using System.IO;
using System.Text;
using Anderson.PackageAudit.PackageModels;
using Anderson.PackageAudit.SharedPipes.Caching;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NUnit.Framework;
using ServiceStack.Redis;

namespace Anderson.PackageAudit.Tests.Handlers
{
    public class CachingTests
    {
        readonly RedisClient _redisClient = new RedisClient("localhost:32768");

        [TearDown]
        public void Setup()
        {
            _redisClient.DeleteAll<TestObject>();
        }

        [Test]
        public void GivenCachingPipe_WhenCallingHandle_ThenResponseIsCached()
        {
            var expected = new Package { name = "any",version = "001"} ;
            var requestHandler = new TestHandler(new[] { expected});
            var pipe = new AuditRequestCachingPipe(_redisClient)
            {
                InnerHandler = requestHandler
            };
            
            pipe.Handle(new AuditRequest
            {
                Packages = new List<ProjectPackages>(new []{new ProjectPackages{Name = "any", Version = "001"}})
            });
            var client = _redisClient.As<Package>();
            var actual = client.GetById("ANY001");
            actual.Should().BeEquivalentTo(expected);
        }
    }
}