using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace PPL.App
{
    public class UpdateRating
    {
        [FunctionName("UpdateRating")]
        public static string FungsiUpdateRating(int RatingScale, string Comment,string LastUpdatedBy, string RatingID) 
        {
            try {
                DateTime Date = DateTime.Now;
                var CreatedDate = Date.ToString("yyyy/MM/dd");
                var LastUpdatedDate = Date.ToString("yyyy/MM/dd");
                var queryUpdateRating = "UPDATE Rating SET RatingScale='" + RatingScale + "', Comment='" + Comment +
                                        "', LastUpdatedBy='" + LastUpdatedBy + "', LastUpdatedDate='" +
                                        LastUpdatedDate + "' WHERE RatingID='" + RatingID + "';";

                SqlDataReader reader = CekEnviVarutkDB.Redirect(queryUpdateRating);
            }
            catch (SqlException e)
            {
                return "Ada Error";
            }
            return "Sukses";
        }
    }
}
