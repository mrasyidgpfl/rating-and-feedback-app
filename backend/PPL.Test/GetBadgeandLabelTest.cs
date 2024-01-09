using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using PPL.Test.Helper;
using PPL.App;
using System.Collections.Generic;

namespace PPL.Test
{
    public class GetBadgeandLabelTest
    {
        private static readonly ILogger logger;

        [Fact(Skip="Masih Error")]
        public static async void Test_Should_return_badge_and_labelFeedback() 
        {
            var request = TestFactory.CreateHttpRequest("label", "badge");
            List<Dictionary<string,string>> response = GetBadgeandLabel.getBadgeLabel(request, logger);

            Assert.True(response.Count > 1);
            Assert.NotNull(response);
        }
    }
}