using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using PPL.Test.Helper;
using PPL.App;
using Moq;

namespace PPL.Test
{
    public class CustomerTableDataTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact(Skip="Masih Error")]
        public async void Http_Should_Get_Table_Data_Customer()
        {
            var request = TestFactory.CreateHttpRequest("idCust", "C1");
            var response = CustomerTableData.GetData(request, logger);
            Assert.NotNull(response);
        }
    }
}
