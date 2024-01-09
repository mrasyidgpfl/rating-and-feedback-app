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
namespace PPL.App
{

    public interface IRekapitulasiSprintTool
    {
        List<string> GetProjectRatingPeriod(string projectId);
        bool IsSprintCompleted(string ratingPeriodId, string personId);
        List<SprintData> TabulateEachSprint(string ratingPeriodId, string personId);
    }


    public class RekapitulasiSprintTool : IRekapitulasiSprintTool
    {
        public static Func<ISQLEmulationClass> SprintFactory = () => new PPL.App.SQLEmulationClass();

        public List<string> GetProjectRatingPeriod(string projectId)
        {
            StringBuilder sb = new StringBuilder("SELECT RatingPeriodID ", 350);
            sb.Append("FROM ProjectRatingPeriod JOIN ProjectRating ");
            sb.Append("ON ProjectRatingPeriod.ProjectID = ProjectRating.ProjectID ");
            sb.Append("WHERE ProjectRatingPeriod.ProjectID = '");
            sb.Append(projectId);
            sb.Append("' ORDER BY PeriodNumber;");
            string query = sb.ToString();
            List<string> result = new List<string>();
            List<List<string>> hasil = SprintFactory.Invoke().ProcessSql(sb.ToString());
            foreach (List<string> item in hasil)
            {
                result.Add(item[0]);
            }

            return result;
        }

        public bool IsSprintCompleted(string ratingPeriodId, string personId)
        {
            StringBuilder sb = new StringBuilder("SELECT * ", 350);
            sb.Append("FROM Rating ");
            sb.Append("WHERE ratingPeriodId = '");
            sb.Append(ratingPeriodId);
            sb.Append("' AND Assignor = '");
            sb.Append(personId);
            sb.Append("' AND RatingScale = 0;");
            string query = sb.ToString();

            List<List<string>> hasil = SprintFactory.Invoke().ProcessSql(sb.ToString());
            foreach (List<string> item in hasil)
            {
                return false;
            }

            return true;
        }

        public Dictionary<String, int> CalculateMissingReview(string projectId)
        {
            StringBuilder sb = new StringBuilder("Select RatingPeriodID ", 350);
            sb.Append("FROM Rating ");
            sb.Append("WHERE RatingScale = 0 AND Assignor = '");
            sb.Append(projectId);
            sb.Append("'");

            Dictionary<String, int> result = new Dictionary<String, int>();

            List<List<string>> queryResult = SprintFactory.Invoke().ProcessSql(sb.ToString());

            foreach (List<string> rowOfData in queryResult)
            {
                string ratingPeriodId = rowOfData[0];
                if (!result.ContainsKey(rowOfData[0]))
                {
                    result.Add(rowOfData[0], 0);
                }
                result[rowOfData[0]] += 1;
            }
            return result;
        }

        public List<SprintData> TabulateEachSprint(string projectId, string personId)
        {
            List<SprintData> result = new List<SprintData>();
            List<List<string>> queryResult = SprintFactory.Invoke().ProcessSql(getQueryString(projectId, personId));
            List<string> periodID = GetProjectRatingPeriod(projectId);
            Dictionary<String, int> missingReview = CalculateMissingReview(projectId);

            foreach (string period in periodID)
            {
                if (IsSprintCompleted(period, personId))
                {
                    SprintData item = new SprintData(null, null);
                    Dictionary<string, int> ratingFromOthers = new Dictionary<string, int>();
                    foreach (List<string> rowOfData in queryResult)
                    {
                        if (rowOfData[3] == period){
                            if (item.sprintEnd == null)
                            {
                                item = new SprintData(rowOfData[7].Split(" ")[0], rowOfData[8].Split(" ")[0]);
                            } 
                            item.expectedNumberOfRating += 1;
                            int scale = Int32.Parse(rowOfData[1]);
                            if (!ratingFromOthers.ContainsKey(rowOfData[0]))
                            {
                                ratingFromOthers.Add(rowOfData[0], scale);
                            }
                            item.AddCriticism(rowOfData[5], rowOfData[6]);
                        }
                    }
                    foreach (KeyValuePair<string, int> entry in ratingFromOthers)
                    {
                        item.RegisterRating(entry.Value);
                    }
                    item.expectedNumberOfRating = ratingFromOthers.Count;
                    if (missingReview.ContainsKey(period))
                    {
                        item.expectedNumberOfRating += missingReview[period];
                    }
                    result.Add(item);
                }
                else
                {
                    result.Add(null);
                }
            }
            return result;
        }

        public String getQueryString(string projectId, string personId)
        {
            StringBuilder sb = new StringBuilder("SELECT RatingID, RatingScale, Assignor", 350);
            sb.Append(", RatingPeriodID, Y.RatingCatFeedbackID, Description, Comment ");
            sb.Append(", StartPeriod, EndPeriod ");
            sb.Append("FROM RatingCatFeedback ");
            sb.Append("JOIN ");
            sb.Append("(SELECT X.RatingID, Assignor, RatingPeriodID, RatingCatFeedbackID, RatingScale, Comment, ");
            sb.Append("StartPeriod, EndPeriod ");
            sb.Append("FROM RatingFeedback ");
            sb.Append("JOIN ");
            sb.Append("(SELECT RatingScale, RatingID, Assignor, Rating.RatingPeriodID,");
            sb.Append(" Comment, StartPeriod, EndPeriod ");
            sb.Append("FROM ProjectRatingPeriod ");
            sb.Append("JOIN Rating ON ");
            sb.Append("ProjectRatingPeriod.RatingPeriodID = Rating.RatingPeriodID ");
            sb.Append("WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("' AND Assignee = '");
            sb.Append(personId);
            sb.Append("' AND  RatingScale > 0) X ON RatingFeedback.RatingID = X.RatingID) Y");
            sb.Append(" ON RatingCatFeedback.RatingCatFeedbackID = Y.RatingCatFeedbackID");

            return sb.ToString();
        }
    }
}
