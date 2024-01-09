using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using PPL.Test.Helper;
using PPL.App;
using Moq;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;


namespace PPL.Test{
    public class BadgeRecapitulationTest{

        private readonly ITestOutputHelper output;
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void Test_Security_Check(){
            BadgeRecapitulationTool obj = new BadgeRecapitulationTool();
            Assert.False(obj.securityCheck(null, null));
            Assert.False(obj.securityCheck(null, "badak"));
            Assert.False(obj.securityCheck("badak", ""));
            Assert.True(obj.securityCheck("badak", "badak"));
            Assert.False(obj.securityCheck("badak", "\"DROP "));
        }

        [Fact]
        public async void Http_Respond_On_Invalid_Data()
        {
            Dictionary<string,StringValues> postParam= new Dictionary<string,StringValues>();
            postParam.Add("ProjectId", "sampleProjectId");

            var request = new DefaultHttpRequest(new DefaultHttpContext()){
                Query = new QueryCollection(postParam)
            };

            var response = (OkObjectResult)await BadgeRecapitulation.Run(request, logger);
            string stringResponse = (String) response.Value;
            Assert.Equal("Bad Input", stringResponse);
        }
	}
}
