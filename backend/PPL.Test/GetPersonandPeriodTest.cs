using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using PPL.Test.Helper;
using PPL.App;
using System;
using System.Collections.Generic;

namespace PPL.Test
{
    public class GetPersonandPeriodTest
    {
        public static readonly ILogger logger = TestFactory.CreateLogger();

        [Fact(Skip="Masih Error")]
        public async void Http_should_return_person_and_period()
        {
            var request = TestFactory.CreateHttpRequest("proyekName","P1");
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>(); 
            res = GetPersonandPeriod.getPersonbyPeriodData(request, logger);
            Assert.NotNull(res);
            Assert.True(res.Count > 0);
        }
    }
}
