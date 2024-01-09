using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace PPL.App
{
    public static class GetBadgeandLabel
    {
        [FunctionName("GetBadgeandLabel")]
        public static List<Dictionary<string, string>> getBadgeLabel([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string kode = req.Query["kode"];
            List<Dictionary<string, string>> getBadgeandLabel = new List<Dictionary<string, string>>();
            Dictionary<string, string> badge = new Dictionary<string, string>();
            Dictionary<string, string> label = new Dictionary<string, string>();

            StringBuilder sb = new StringBuilder("SELECT RatingCatFeedbackID, Description, ImageURL ");
            sb.Append("FROM RatingCatFeedback ");
            if (kode.Equals("dev")) {
                sb.Append("WHERE RatingCategoryID = 1; ");
            }
            else {
                sb.Append("WHERE RatingCategoryID = 2; ");
            }
            SqlDataReader reader = CekEnviVarutkDB.Redirect(sb.ToString());

            while(reader.Read()) {
                
                string RatingCatFeedbackID = reader.GetString(0);
                string urlBadgeImage = reader.GetString(2);
                string labelFeedback = reader.GetString(1); 

                if (!label.ContainsKey(RatingCatFeedbackID) && !labelFeedback.Equals("") && urlBadgeImage.Equals("")) {
                    label.Add(RatingCatFeedbackID, labelFeedback);
                }
                else if (!badge.ContainsKey(RatingCatFeedbackID) && urlBadgeImage.Contains("https")) {
                    string badgeAndurl = labelFeedback+","+urlBadgeImage;
                    badge.Add(RatingCatFeedbackID, badgeAndurl);
                }
            }

            getBadgeandLabel.Add(label);getBadgeandLabel.Add(badge);

            return getBadgeandLabel;
        }
    }
}
