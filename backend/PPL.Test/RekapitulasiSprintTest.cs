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
    public class RekapitulasiSprintTest{

        private readonly ITestOutputHelper output;
        private readonly ILogger logger = TestFactory.CreateLogger();

        
        [Fact]
        public async void Test_SprintData(){
            SprintData testObject = new SprintData("6/12/2019", "8/12/2019");
            testObject.AddCriticism("disiplin", "yang disiplin dong");
            Assert.Equal(1, testObject.commentaryList["disiplin"].Count);
        }

        [Fact]
        public async void Test_RekapSprint_SprintNotCompleted()
        {
            List<List<string>> firstMockReturn = new List<List<string>>();
            List<string> temporary = new List<string>();
            temporary.Add("something");
            firstMockReturn.Add(temporary);

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(firstMockReturn);          

            RekapitulasiSprintTool.SprintFactory = () => mock.Object; 

            RekapitulasiSprintTool o = new RekapitulasiSprintTool();
            bool result = o.IsSprintCompleted("sampleProjectId", "samplePersonId");
            Assert.False(result);            
        }

        [Fact]
        public async void Test_RekapSprint_SprintCompleted()
        {
            List<List<string>> secondMockReturn = new List<List<string>>();

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(secondMockReturn);          

            RekapitulasiSprintTool.SprintFactory = () => mock.Object; 

            RekapitulasiSprintTool o = new RekapitulasiSprintTool();
            bool result= o.IsSprintCompleted("sampleProjectId", "samplePersonId");
            Assert.True(result);            
        }
        
        [Fact]
        
        public async void Test_RekapSprint_GetProjectRatingPeriod()
        {
            List<List<string>> firstMockReturn = new List<List<string>>();
            List<string> temporary = new List<string>();
            temporary.Add("Sprint 1");
            firstMockReturn.Add(temporary);
            temporary = new List<string>();
            temporary.Add("Sprint 2");
            firstMockReturn.Add(temporary);

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(firstMockReturn);          

            RekapitulasiSprintTool.SprintFactory = () => mock.Object; 

            RekapitulasiSprintTool o = new RekapitulasiSprintTool();
            List<string> result= o.GetProjectRatingPeriod("sampleProjectId");
            Assert.Equal(2, result.Count);            
        }

        [Fact]
        public async void RekapSprint_Http_Respond_On_Invalid_Data()
        {
            Dictionary<string,StringValues> postParam= new Dictionary<string,StringValues>();
            postParam.Add("ProjectId", "sampleProjectId");

            var request = new DefaultHttpRequest(new DefaultHttpContext()){
                Query = new QueryCollection(postParam)
            };

            var response = (OkObjectResult)await RekapitulasiSprint.Run(request, logger);
            string stringResponse = (String) response.Value;
            Assert.Equal("Bad Input", stringResponse);
        }
	}
}
