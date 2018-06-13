using System.Collections.Generic;
using System.IO;
using System.Text;
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
            var requestHandler = new TestHandler();
            var pipe = new CachingPipe<TestObject, TestObject>(_redisClient)
            {
                InnerHandler = requestHandler
            };

            var expected = new[] {new TestObject {Id = "someid"}};
            pipe.Handle(new List<TestObject>(expected));
            requestHandler.Input.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GivenCachingPipe_WhenCallingHandleWithCeachedResponse_ThenRequestHasCachedResponsesFiltered()
        {
            var requestHandler = new TestHandler();
            var pipe = new CachingPipe<TestObject, TestObject>(_redisClient)
            {
                InnerHandler = requestHandler
            };

            _redisClient.Store(new TestObject {Id = "cachedId"});

            var expected = new[] { new TestObject { Id = "someid" } };
            var testObjects = new List<TestObject>(expected)
            {
                new TestObject { Id = "cachedId" }
            };

            pipe.Handle(testObjects);
            requestHandler.Input.Should().BeEquivalentTo(expected);
        }

        private static DefaultHttpRequest CreateDefaultHttpRequest()
        {
            var httpRequest = new DefaultHttpRequest(new DefaultHttpContext());
            var memoryStream = new MemoryStream();

            var request = "[{name: \"FluentValidation\", version: \"1.0.1\"}]";
            memoryStream.Write(Encoding.UTF8.GetBytes(request), 0, request.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            httpRequest.Body = memoryStream;
            return httpRequest;
        }
    }
}