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
    public class HomePageTest{

        private readonly ITestOutputHelper output;
        private readonly ILogger logger = TestFactory.CreateLogger();


        [Fact]
        public async void Test_Security_Check(){
            HomePageTool obj = new HomePageTool();
            Assert.False(obj.securityCheck(null));
            Assert.True(obj.securityCheck("badak"));
            Assert.False(obj.securityCheck(""));
            Assert.False(obj.securityCheck("\"DROP "));
        }

        [Fact(Skip="Masih Error")]

        public async void test_GetYourProject()
        {
            List<List<string>> firstMockReturn = new List<List<string>>();
            List<string> temporary = new List<string>();
            temporary.Add("PT Proyek");
            temporary.Add("ProjectID");
            temporary.Add("ProjectName");
            temporary.Add("10/25/2018");
            temporary.Add("12/10/2019");
            temporary.Add("true");
            firstMockReturn.Add(temporary);

            var mock = new Mock<ISQLEmulationClass>();
            // TODO : fix this
            //mock.Setup(f => f.ProcessProjectSql(It.IsAny<string>())).Returns(firstMockReturn);          

            HomePageTool.HomePageFactory  = () => mock.Object; 

            IHomePageTool o = new HomePageTool();
            List<HomePageData> result= o.GetYourProject("personId");
            Assert.Equal(1, result.Count);  
        }



        [Fact]
        public async void test_GetYourRating()
        {
            List<List<string>> firstMockReturn = new List<List<string>>();
            List<string> temporary = new List<string>();
            temporary.Add("4");
            firstMockReturn.Add(temporary);
            temporary = new List<string>();
            temporary.Add("0");
            firstMockReturn.Add(temporary);
            temporary = new List<string>();
            temporary.Add("6");
            firstMockReturn.Add(temporary);

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(firstMockReturn);          

            HomePageTool.HomePageFactory  = () => mock.Object; 

            IHomePageTool o = new HomePageTool();
            float result= o.GetYourRating("projectId", "personId");
            Assert.Equal(5, result);  
        }
    }
}
