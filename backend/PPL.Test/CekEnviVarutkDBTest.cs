using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using PPL.Test.Helper;
using PPL.App;

namespace PPL.Test
{
    public class CekEnviVarutkDBTest
    {
        [Fact(Skip="Masih Error")]
        public async void Http_Should_Redirect_to_DB()
        {
            var query = "SELECT * FROM RATING";
            SqlDataReader hasilquery = CekEnviVarutkDB.Redirect(query);
            Assert.NotNull(hasilquery);
        }
        [Fact(Skip="Masih Error")]
        public async void DB_To_Test_CountFields_In_A_Table()
        {
            var query = "SELECT * FROM RATING";
            SqlDataReader hasilquery = CekEnviVarutkDB.Redirect(query);
            Assert.True(hasilquery.FieldCount > 3);
        }

        [Fact]
        public async void Change_Date_Format_Test() {
            string date = "3/17/2019";
            string res = CekEnviVarutkDB.changeDate(date);
            Assert.True(res.Contains("March"));
        }
    }
}
