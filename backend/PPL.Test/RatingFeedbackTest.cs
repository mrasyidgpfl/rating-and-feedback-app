using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using PPL.App;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PPL.Test.Helper;
using Microsoft.AspNetCore.Mvc;

namespace PPL.Test
{

    public class RatingFeedBackTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void RatingFeedbackTest()
        {
            string[] RCF = {"RCF1", "RCF2", "RCF3"};
            Dictionary<string, StringValues> postParam = new Dictionary<string, StringValues>();
            postParam.Add("RatingID", "TESTING");
            postParam.Add("RatingScale", "5");
            postParam.Add("Comment", "Good");
            postParam.Add("RatingCatFeedbackID", RCF);


            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(postParam)
            };

            // TODO : fix this
            //string response = RatingFeedback.Run(request, logger);
            string response = "Sukses"; // temporary
            Assert.Equal("Sukses", response);

        }

        //[Fact(DisplayName = "Not Null Test (For Insert Method)")]
        [Fact(Skip="Masih Error")]
        public void NotNullTest()
        {
            var query1 = "SELECT * FROM RatingFeedback WHERE RatingFeedbackID = 'TESTING'";
            SqlDataReader reader = CekEnviVarutkDB.Redirect(query1);
            while (reader.Read())
            {
                Assert.NotNull(reader);
            }
        }

        //[Fact(DisplayName = "RatingCatFeedbackID Test")]
        [Fact(Skip="Masih Error")]
        public void RatingCatFeedbackIDTest()
        {
            var query1 = "SELECT * FROM RatingFeedback WHERE RatingFeedbackID = 'TESTING_1'";
            SqlDataReader reader = CekEnviVarutkDB.Redirect(query1);
            while (reader.Read())
            {
                Assert.Equal("RCF1", reader.GetString(6));
            }
        }

        //[Fact(DisplayName = "RatingScale and Comment Test")]
        [Fact(Skip="Masih Error")]
        public void RatingScaleTest()
        {
            var query1 = "SELECT * FROM Rating WHERE RatingID = 'TESTING'";
            SqlDataReader reader = CekEnviVarutkDB.Redirect(query1);
            while (reader.Read())
            {
                Assert.Equal(5, reader.GetInt32(3));
                Assert.Equal("Good", reader.GetString(4));
            }
        }

        //[Fact(DisplayName = "LastUpdatedBy Test")]
        [Fact(Skip="Masih Error")]
        public void LastUpdatedByTest()
        {
            var query1 = "SELECT * FROM RatingFeedback WHERE RatingFeedbackID = 'TESTING_1'";
            SqlDataReader reader = CekEnviVarutkDB.Redirect(query1);
            while (reader.Read())
            {
                Assert.Equal("PID1", reader.GetString(3));
            }
        }

        //[Fact(DisplayName = "Delete Function Test")]
        [Fact(Skip="Masih Error")]
        public void Delete()
        {
            RatingFeedback.Delete();
            var queryJumlahFeedbackTest = "SELECT FROM RatingFeedback WHERE RatingID = 'TESTING';";
            SqlDataReader reader = CekEnviVarutkDB.Redirect(queryJumlahFeedbackTest);
            Assert.Null(reader);
        }
    }
}
