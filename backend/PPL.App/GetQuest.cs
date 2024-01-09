using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PPL.App
{
    public class GetQuest
    {

        [FunctionName("GetPeriodforProject")]
        public static string GetPeriod([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string kodeRating = req.Query["KodeRating"];
            string res = "";
            SqlDataReader reader = null;

            StringBuilder sb = new StringBuilder("SELECT StartPeriod, EndPeriod, ProjectName, RatingID, ClientName ");
            sb.Append("FROM ProjectRatingPeriod, Rating , Project, Client ");
            sb.Append("WHERE Rating.ClientID = Client.ClientID AND Rating.RatingID = '");
            sb.Append(kodeRating);
            sb.Append("' AND RatingScale = 0 AND ProjectRatingPeriod.RatingPeriodID = Rating.RatingPeriodID AND ProjectRatingPeriod.ProjectID = Project.ProjectID; ");
            string query = sb.ToString();
            
            reader = CekEnviVarutkDB.Redirect(query);

            try 
            {

                while (reader.Read()) {
                    string period = reader.GetDateTime(0).ToShortDateString()
                            +" - "+reader.GetDateTime(1).ToShortDateString();
                    string proyekName = "#"+reader.GetString(2);
                    string ratingID = "#"+reader.GetString(3);
                    string clientName = "#"+reader.GetString(4);
                    res = period+proyekName+ratingID+clientName;
                }
                return res;
            }
             catch (SqlException e)
            {
                return "Has Filled Rating";
            }
            return res;
        }
    }
}
