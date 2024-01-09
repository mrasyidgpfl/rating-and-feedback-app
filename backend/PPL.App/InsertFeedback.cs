using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using System.Data.SqlClient;

namespace PPL.App
{
    public class InsertFeedback
    {

        [FunctionName("InsertFeedback")]
        public static string InsertRatingFeedback(string RatingID, string CreatedBy, string LastUpdatedBy, string[] RatingCatFeedbackID)
        {
            try {
                DateTime Date = DateTime.Now;
                int jumlahLabelTerpilih = RatingCatFeedbackID.Length;
                DateTime Date2 = GetDate(RatingID);
                var CreatedDate = Date2.ToString("yyyy/MM/dd"); 
                var LastUpdatedDate = Date.ToString("yyyy/MM/dd");

                for (int i=1; i < jumlahLabelTerpilih+1; i++) {
                    var queryRatingFeedback = "INSERT INTO RatingFeedback(RatingFeedbackID, RatingID," +
                        " CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate, RatingCatFeedbackID) VALUES('" + RatingID + "_" + i +
                        "', '" + RatingID + "', '" + CreatedBy + "', '" + CreatedDate + "', '" + LastUpdatedBy + "', '" + LastUpdatedDate +
                        "', '" + RatingCatFeedbackID[i-1] + "'); ";
                    SqlDataReader reader = CekEnviVarutkDB.Redirect(queryRatingFeedback);
                }
            }
            catch (SqlException e) {
                return "Ada Error";
            }
            return "Sukses";
        }

        [FunctionName("GetDate")]
        public static DateTime GetDate(string RatingID)
        {
            DateTime CreatedDate = new DateTime();
            var queryCreatedDate = "SELECT * FROM Rating WHERE RatingID = '" + RatingID + "';";
            SqlDataReader hasilQuery = CekEnviVarutkDB.Redirect(queryCreatedDate);
            while (hasilQuery.Read())
            {
                CreatedDate = hasilQuery.GetDateTime(6);
                return CreatedDate;
            }
            return CreatedDate;
        }
    }
}
