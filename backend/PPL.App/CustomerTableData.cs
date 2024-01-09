using System;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PPL.App
{
    public static class CustomerTableData
    {

        [FunctionName("GetCustomerDataExceptLabel")]
        public static List<Dictionary<string,string>> GetData([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<Dictionary<string,string>> res = new List<Dictionary<string,string>>();
            Dictionary<string, string> tempDict = new Dictionary<string, string>();
            string temp = "";
            string idCust = req.Query["idCust"];
            string query = "SELECT P.ProjectName, C.ClientName, PRP.PeriodNumber, PRP.StartPeriod, PRP.EndPeriod, "+
                           "R.RatingScale, R.Comment FROM Project P, Client C, ProjectRatingPeriod PRP, Rating R WHERE "+
                           "P.ProjectID = PRP.ProjectID AND R.RatingPeriodID = PRP.RatingPeriodID AND R.ClientID = '"+
                           idCust+"' AND R.ClientID = C.ClientID; ";
            SqlDataReader reader = CekEnviVarutkDB.Redirect(query);
            List<string> arrTemp1 = new List<string>();
            int index = 0;
            while(reader.Read()) {
                temp+=reader.GetString(0)+"#"+reader.GetString(1)+"#"+"Period "+reader.GetInt32(2)+"#"+
                      reader.GetDateTime(3).ToShortDateString()
                      +" - "+reader.GetDateTime(4).ToShortDateString()+"#"+reader.GetInt32(5)+"#";
                string cek = reader.GetString(6).Equals("")? temp+="kosong" : temp+=reader.GetString(6);
                string tempstr = index+"";
                tempDict.Add(tempstr, temp);
                index+=1;
            }
            res.Add(tempDict);
            res.Add(getBadgeFeedback(idCust));

            return res;
        }

        [FunctionName("GetBadgeFeedbackCustomer")]
        public static Dictionary<string, string> getBadgeFeedback(string idCusto) {
            
            string idCust = idCusto;

            Dictionary<string, string> res = new Dictionary<string, string>();
            string query = "SELECT PRP.PeriodNumber, RCF.Description, RCF.ImageURL, RCF.FeedbackType FROM "+
                 "RatingCatFeedback RCF, ProjectRatingPeriod PRP, Rating R, RatingFeedback RF WHERE RCF.RatingCategoryID = '"
                 +2+"' AND R.ClientID = '"+idCust+
                 "' AND R.RatingID = RF.RatingID AND RF.RatingCatFeedbackID = RCF.RatingCatFeedbackID AND "+
                 "PRP.RatingPeriodID = R.RatingPeriodID; ";

            SqlDataReader reader = CekEnviVarutkDB.Redirect(query);
            while(reader.Read()) {

                string period = "Period "+reader.GetInt32(0);
                string tempstr = "";
                if (reader.GetString(3).Equals("1")) { // If return type is badge
                    tempstr+=reader.GetString(1)+"#"+reader.GetString(2);
                }
                else if (reader.GetString(3).Equals("2")) { // If return type is feedback
                    tempstr+=reader.GetString(1)+"#";
                }
                if (res.ContainsKey(period)) {
                    tempstr = res[period]+tempstr;
                    res.Remove(period);
                    res.Add(period, tempstr);
                }
                else {
                    res.Add(period, tempstr);
                }
            }
            return res;
        }
    }
}
