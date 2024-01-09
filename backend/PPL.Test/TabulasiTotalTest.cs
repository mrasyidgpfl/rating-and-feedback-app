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
    public class TabulasiTotalTest{

        private readonly ITestOutputHelper output;
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void Test_Security_Check(){
            TabulasiTotalTool obj = new TabulasiTotalTool();
            Assert.False(obj.securityCheck(null, null));
            Assert.False(obj.securityCheck(null, "badak"));
            Assert.False(obj.securityCheck("badak", ""));
            Assert.True(obj.securityCheck("badak", "badak"));
            Assert.False(obj.securityCheck("badak", "\"DROP "));
        }

        [Fact]

        public async void Test_Tabulate()
        {
            List<List<string>> firstMockReturn = new List<List<string>>();
            List<string> temporary = new List<string>();
            temporary.Add("5");
            temporary.Add("samplePersonId");
            firstMockReturn.Add(temporary);
            temporary = new List<string>();
            temporary.Add("6");
            temporary.Add("sampleNotPersonId");
            firstMockReturn.Add(temporary);

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(firstMockReturn);          

            TabulasiTotalTool.Factory = () => mock.Object; 

            TabulasiTotalTool o = new TabulasiTotalTool();
            IDictionary<string,int> result= o.Tabulate("sampleProjectId", "samplePersonId");
            Assert.Equal(11, result["team total"]);           
            Assert.Equal(2, result["team count"]);  
            Assert.Equal(5, result["personal total"]);           
            Assert.Equal(1, result["personal count"]);           
        }


        [Fact]

        public async void Test_SprintNotCompleted()
        {
            List<List<string>> firstMockReturn = new List<List<string>>();
            List<string> temporary = new List<string>();
            temporary.Add("something");
            firstMockReturn.Add(temporary);

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(firstMockReturn);          

            TabulasiTotalTool.Factory = () => mock.Object; 

            TabulasiTotalTool o = new TabulasiTotalTool();
            bool result = o.IsSprintCompleted("sampleProjectId", "samplePersonId");
            Assert.False(result);            
        }

        public async void Test_SprintCompleted()
        {
            List<List<string>> firstMockReturn = new List<List<string>>();

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(firstMockReturn);          

            TabulasiTotalTool.Factory = () => mock.Object; 

            TabulasiTotalTool o = new TabulasiTotalTool();
            bool result= o.IsSprintCompleted("sampleProjectId", "samplePersonId");
            Assert.True(result);            
        }
        
        [Fact]
        
        public async void Test_GetProjectRatingPeriod()
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

            TabulasiTotalTool.Factory = () => mock.Object; 

            TabulasiTotalTool o = new TabulasiTotalTool();
            List<string> result= o.GetProjectRatingPeriod("sampleProjectId");
            Assert.Equal(2, result.Count);            
        }

        [Fact(Skip="Masih Error")]

        public async void Http_Respond_On_Valid_Data()
        {
            List<string> firstMockReturn = new List<string>();
            firstMockReturn.Add("Sprint 1");
            firstMockReturn.Add("Sprint 2");

            var mock = new Mock<ITabulasiTotalTool>();

            mock.Setup(f => f.GetProjectRatingPeriod(It.IsAny<string>())).Returns(firstMockReturn);
            mock.SetupSequence(f => f.IsSprintCompleted(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false)
            .Returns(true);
             

            Dictionary<string, int> secondMockReturn = new Dictionary<string, int>();
            secondMockReturn.Add("personal count", 4);
            secondMockReturn.Add("personal total", 21);
            secondMockReturn.Add("team count", 100);
            secondMockReturn.Add("team total", 500);
            mock.Setup(f => f.Tabulate(It.IsAny<string>(),It.IsAny<string>())).Returns(secondMockReturn);

            mock.Setup(f => f.GetProjectName(It.IsAny<string>())).Returns("Project");

            TabulasiTotal.Factory = () => mock.Object;

            Dictionary<string,StringValues> postParam= new Dictionary<string,StringValues>();
            postParam.Add("ProjectId", "sampleProjectId");
            postParam.Add("PersonId", "samplePersonId");
            
            var request = new DefaultHttpRequest(new DefaultHttpContext()){
                Query = new QueryCollection(postParam)
            };

            var response = (OkObjectResult)await TabulasiTotal.Run(request, logger);
            string stringResponse = (String) response.Value;
            Assert.Equal("{\n  \"personalAverage\": 5.25,\n  \"teamAverage\": 5.0,\n \"name\": \"Project\"\n}", 
            stringResponse);
        }

        [Fact]
        public async void Http_Respond_On_Invalid_Data()
        {
            Dictionary<string,StringValues> postParam= new Dictionary<string,StringValues>();
            postParam.Add("ProjectId", "sampleProjectId");

            var request = new DefaultHttpRequest(new DefaultHttpContext()){
                Query = new QueryCollection(postParam)
            };

            var response = (OkObjectResult)await TabulasiTotal.Run(request, logger);
            string stringResponse = (String) response.Value;
            Assert.Equal("Bad Input", stringResponse);
        }
	}
}
