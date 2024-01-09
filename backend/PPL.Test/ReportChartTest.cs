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
    public class ReportChartTest{

        private readonly ITestOutputHelper output;
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]

        public async void Test_GetProjectRatingPeriod(){
            List<List<string>> firstMockReturn = new List<List<string>>();
            List<string> temporary = new List<string>();
            temporary.Add("sampleProjectId1");
            firstMockReturn.Add(temporary);
            temporary = new List<string>();
            temporary.Add("sampleProjectId2");
            firstMockReturn.Add(temporary);

            var mock = new Mock<ISQLEmulationClass>();
            mock.Setup(f => f.ProcessSql(It.IsAny<string>())).Returns(firstMockReturn);          

            ReportChartTool.Factory = () => mock.Object; 

            IReportChartTool o = new ReportChartTool();
            List<string> result = o.GetProjectRatingPeriod("sampleProjectId");
            Assert.Equal(2, result.Count);                     
        }
    }
}
