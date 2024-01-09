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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace PPL.App{
    
    public interface IBadgeRecapitulationTool{
        List<string> GetFeedbackCategory(string projectId);
        List<string> GetProjectRatingPeriod(string projectId);
        bool IsSprintCompleted(string ratingPeriodId, string personId);
        IDictionary<string, int> CollectBadge(string ratingPeriodId, string personId);
        bool securityCheck(string projectId, string personId);
    }

    public class BadgeRecapitulationTool:IBadgeRecapitulationTool{
        public static Func<ISQLEmulationClass> BadgeFactory  = () => new PPL.App.SQLEmulationClass();

        public bool securityCheck(string projectID, string personId){

            Regex r = new Regex("^[a-zA-Z0-9]*$");

            if(projectID == null){
                return false;
            }
            if(personId == null){
                return false;
            }
            if(projectID.Length == 0){
                return false;
            }
            if(personId.Length == 0){
                return false;
            }
            if (! r.IsMatch(projectID)) {
                return false;
            }
            if(! r.IsMatch(personId)){
                return false;
            }
            return true;
        }

        public List<string> GetFeedbackCategory(string projectId){
            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder("SELECT TeamRatingCatID, CustomerRatingCatID ", 400);
            sb.Append("FROM ProjectRating WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("';");
            string teamRatingCategory, customerRatingCategory;
            List<List<string>> hasil = BadgeFactory.Invoke().ProcessSql(sb.ToString()); // Zeroth mock
            teamRatingCategory = hasil[0][0];
            customerRatingCategory = hasil[0][1];

            // Mencari badge dari peer rating

            sb = new StringBuilder("SELECT ImageUrl ");
            sb.Append("FROM RatingCatFeedback ");
            sb.Append("WHERE FeedbackType=1 ");
            sb.Append("AND RatingCategoryID ='");
            sb.Append(teamRatingCategory);
            sb.Append("';");
            hasil = BadgeFactory.Invoke().ProcessSql(sb.ToString()); // First mock
            foreach(List<string> item in hasil){
                result.Add(item[0]);
            }
        
            sb = new StringBuilder("SELECT ImageUrl ");
            sb.Append("FROM RatingCatFeedback ");
            sb.Append("WHERE FeedbackType=1 ");
            sb.Append("AND RatingCategoryID ='");
            sb.Append(customerRatingCategory);
            sb.Append("';");
            hasil = BadgeFactory.Invoke().ProcessSql(sb.ToString()); // 1.5th mock
            foreach(List<string> item in hasil){
                result.Add(item[0]);
            }
            return result;
        }

        public List<string> GetProjectRatingPeriod(string projectId){
            StringBuilder sb = new StringBuilder("SELECT RatingPeriodID ", 350);
            sb.Append("FROM ProjectRatingPeriod JOIN ProjectRating ");
            sb.Append("ON ProjectRatingPeriod.ProjectID = ProjectRating.ProjectID ");
            sb.Append("WHERE ProjectRatingPeriod.ProjectID = '");
            sb.Append(projectId);
            sb.Append("';");
            string query = sb.ToString();
            List<string> result = new List<string>();
            List<List<string>> hasil = BadgeFactory.Invoke().ProcessSql(sb.ToString());// second mock
            foreach(List<string> item in hasil){
                result.Add(item[0]);
            }
            return result;
        }

        public bool IsSprintCompleted(string ratingPeriodId, string personId){
            StringBuilder sb = new StringBuilder("SELECT * ", 350);
            sb.Append("FROM Rating ");
            sb.Append("WHERE ratingPeriodId = '");
            sb.Append(ratingPeriodId);
            sb.Append("' AND Assignor = '");
            sb.Append(personId);
            sb.Append("' AND RatingScale = 0;");
            string query = sb.ToString();
            List<List<string>> hasil = BadgeFactory.Invoke().ProcessSql(sb.ToString()); // third and fourth mock
            foreach(List<string> item in hasil){
                return false;
            }
            return true;
        }

        public IDictionary<string, int> CollectBadge(string ratingPeriodId, string personId){
            StringBuilder sb = new StringBuilder("SELECT ImageUrl ", 500);
            sb.Append("FROM Rating R JOIN ");
            sb.Append("(SELECT ImageUrl, SeqNo, FeedbackType, RatingID ");
            sb.Append("FROM RatingFeedback RF JOIN RatingCatFeedback RCF ");
            sb.Append("ON RF.RatingCatFeedbackID = RCF.RatingCatFeedbackID) X ");
            sb.Append("ON R.RatingID = X.RatingID ");
            sb.Append("WHERE FeedbackType = 1");
            sb.Append(" AND Assignee = '");
            sb.Append(personId);
            sb.Append("' AND RatingPeriodID = '");
            sb.Append(ratingPeriodId);
            sb.Append("' ORDER BY SeqNo;");
            string query = sb.ToString();
            List<List<string>> hasil = BadgeFactory.Invoke().ProcessSql(query); // fifth mock
            IDictionary<string, int> compilation = new Dictionary<string, int>();
            foreach(List<string> item in hasil){
                if (compilation.ContainsKey(item[0])){
                    compilation[item[0]] = compilation[item[0]] + 1;
                }
                else{
                    compilation.Add(item[0], 1);
                }
            }
            return compilation;
        }
    }
}
