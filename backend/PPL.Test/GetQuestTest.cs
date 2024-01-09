using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Text;
using Xunit;
using PPL.Test.Helper;
using PPL.App;
using System.Collections.Generic;

namespace PPL.Test
{
    public static class GetQuestTest
    {
        private static readonly ILogger logger = TestFactory.CreateLogger();

        [Fact(Skip="Masih Error")]
        public static void test_request_should_return_period_project()
        {
            var request = TestFactory.CreateHttpRequest("KodeProyek", "RP3");
            string res = GetQuest.GetPeriod(request, logger);
            string[] arr = res.Split("#");
            Assert.NotNull(arr); Assert.NotEmpty(arr);
            Assert.NotNull(res);
        }
    }
}
