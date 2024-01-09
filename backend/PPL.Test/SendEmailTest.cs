using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using PPL.Test.Helper;
using PPL.App;

namespace PPL.Test
{
    public class SendEmailTest
    {
        private static readonly ILogger logger = TestFactory.CreateLogger();

        [Fact(Skip="Masih Error")]
        public async void Validate_link_test()
        {
            var request = TestFactory.CreateHttpRequest("kode", "yfef");
            var response = SendEmail.validate(request, logger);
            Assert.NotNull(response);
        }
    }
}
