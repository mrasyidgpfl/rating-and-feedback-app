using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using PPL.Test.Helper;
using PPL.App;

namespace PPL.Test
{
    public class HelloWorldTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void Http_Trigger_Should_Return_Hello_World()
        {
            var request = TestFactory.CreateHttpRequest("name", "hello");
            var response = (OkObjectResult)await HelloWorld.Run(request, logger);
            Assert.Equal("Hello, World!", response.Value);
        }
    }
}

