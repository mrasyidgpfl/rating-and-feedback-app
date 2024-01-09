using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace PPL.App
{
    public class feedback {
        public string rating_id;
        public string scale;
        public string comment;
        public string[] rcf;
    }
    public class RatingFeedback
    {

    [FunctionName("RatingFeedback")]
        public static Dictionary<string, string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Dictionary<string, string> res  = new Dictionary<string, string>();
            try
            {
                
                StreamReader read = new StreamReader(req.Body);
                var request = read.ReadToEnd();
                var post = JsonConvert.DeserializeObject<feedback>(request);
                string RatingID = post.rating_id;
                int RatingScale = Int32.Parse(post.scale);
                string Comment = post.comment;
                string[] RatingCatFeedbackID = post.rcf;

                if (RatingScale == 0 || RatingCatFeedbackID == null)
                {
                    res.Add("hasil","Maaf, parameter yang diberikan tidak lengkap");
                }
                else
                {
                    if (Comment == null) {
                        Comment = "";
                    }
                    string[] names = GetNames(RatingID);
                    InsertFeedback.InsertRatingFeedback(RatingID, names[0], names[1], RatingCatFeedbackID);
                    UpdateRating.FungsiUpdateRating(RatingScale, Comment, names[1], RatingID);
                    
                    res.Add("hasil","Sukses");
                }
                return res;

            }
            catch (SqlException e)
            {
                res.Add("hasil","Error");
                return res;
            }
            res.Add("hasil","Sukses");

            return res;
        }

        [FunctionName("GetNames")]
        public static string[] GetNames(string RatingID)
        {
            string LastUpdatedBy = "";
            string CreatedBy = "";
            string[] names = new string[2];
            var queryCreatedBy = "SELECT * FROM Rating WHERE RatingID = '" + RatingID + "';";
            SqlDataReader hasilQuery = CekEnviVarutkDB.Redirect(queryCreatedBy);
            while (hasilQuery.Read())
            {
                CreatedBy = hasilQuery.GetString(5);
                LastUpdatedBy = hasilQuery.GetString(1);
                names[0] = CreatedBy;
                names[1] = LastUpdatedBy;
            }
            return names;
        }

        [FunctionName("Delete")]
        public static string Delete()
        {
            try {
                var queryHapusFeedbackTest = "DELETE FROM RatingFeedback WHERE RatingID = 'TESTING';";
                SqlDataReader reader = CekEnviVarutkDB.Redirect(queryHapusFeedbackTest);
            }
            catch (SqlException e) {
                return "Ada Error";
            }
            return "Sukses";
        }
    }
}
