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

    public interface IManagerReportBreakdownTool
    {
        List<string> GetProjectRatingPeriod(string projectId);
        List<ManagerReportData> Tabulate(string projectId);
        string GetName(string personId);
        Dictionary<string, double> CalculateAverageRating(Dictionary<string, Dictionary<string, int>> tabulationResult);
        string GetQuery(string projectId);
    }


    public class ManagerReportBreakdownTool : IManagerReportBreakdownTool
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

        public List<ManagerReportData> Tabulate(string projectId)
        {
            List<List<string>> queryResult = SprintFactory.Invoke().ProcessSql(GetQuery(projectId));
            List<ManagerReportData> result = new List<ManagerReportData>();
            List<string> periodID = GetProjectRatingPeriod(projectId);

            foreach (string period in periodID)
            {
                ManagerReportData sprintTabulation = new ManagerReportData();
                Dictionary<string, Dictionary<string, int>> ratingFromOthers = new Dictionary<string, Dictionary<string, int>>();
                foreach (List<string> rowOfData in queryResult)
                {
                    if (rowOfData[3] == period)
                    {
                        if (rowOfData[2] != "")
                        {
                            if (rowOfData[4] != "")
                            {
                                sprintTabulation.RegisterCategoryCriticism(rowOfData[4]);
                            }
                            if (!ratingFromOthers.ContainsKey(rowOfData[2]))
                            {
                                ratingFromOthers.Add(rowOfData[2], new Dictionary<string, int>());
                            }
                            if (!ratingFromOthers[rowOfData[2]].ContainsKey(rowOfData[0]))
                            {
                                ratingFromOthers[rowOfData[2]].Add(rowOfData[0], Int32.Parse(rowOfData[1]));
                            }
                        }
                    }
                }
                Dictionary<string, double> tabulationResult = CalculateAverageRating(ratingFromOthers);
                foreach (KeyValuePair<string, double> entry in tabulationResult)
                {
                    sprintTabulation.RegisterEmployee(entry.Key, GetName(entry.Key), entry.Value);
                }
                result.Add(sprintTabulation);
            }
            return result;
        }

        public string GetName(string personId)
        {
            StringBuilder sb = new StringBuilder("SELECT PersonName ", 350);
            sb.Append("FROM Person ");
            sb.Append("WHERE PersonID ='");
            sb.Append(personId);
            sb.Append("';");
            List<List<string>> queryResult = SprintFactory.Invoke().ProcessSql(sb.ToString());
            return queryResult[0][0];
        }

        public Dictionary<string, double> CalculateAverageRating(Dictionary<string, Dictionary<string, int>> tabulationResult)
        {
            /* calculateAverageRating dipakai untuk menghitung rata-rata nilai setiap employee dalam
            satu periode

            input:
                Dictionary<string, Dictionary<string,int>>
                - key:
                    sebuah string yang merupakan PID employee tersebut
                - value:
                    Dictionary<string, int> dengan spesifikasi beriku
                        - key:
                            ratingID untuk employee tersebut
                        - value:
                            ratingScale yang diberikan dalam ratingID tersebut

            output:
                Dictionary<string, float>
                -key:
                    sebuan string yang merupaka PID employee tersebut
                -value:
                    rata-rata rating yang dimilikinya 
             */
            Dictionary<string, double> result = new Dictionary<string, double>();
            foreach (KeyValuePair<string, Dictionary<string, int>> entry in tabulationResult)
            {
                double total = 0;
                foreach (KeyValuePair<string, int> rating in entry.Value)
                {
                    total += rating.Value * 1.0;
                }
                double average = 0;
                if (entry.Value.Count > 0)
                {
                    average = total / entry.Value.Count;
                }
                result.Add(entry.Key, average);
            }
            return result;
        }

        public string GetQuery(string projectId)
        {
            StringBuilder sb = new StringBuilder("SELECT RatingID, RatingScale, Assignee", 350);
            sb.Append(", RatingPeriodID, Description ");
            sb.Append("FROM RatingCatFeedback ");
            sb.Append("JOIN ");
            sb.Append("(SELECT X.RatingID, Assignee, RatingPeriodID, RatingCatFeedbackID, RatingScale ");
            sb.Append("FROM RatingFeedback ");
            sb.Append("JOIN ");
            sb.Append("(SELECT RatingScale, RatingID, Assignee, Rating.RatingPeriodID ");
            sb.Append("FROM ProjectRatingPeriod ");
            sb.Append("JOIN Rating ON ");
            sb.Append("ProjectRatingPeriod.RatingPeriodID = Rating.RatingPeriodID ");
            sb.Append("WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("'");
            sb.Append(") X ON RatingFeedback.RatingID = X.RatingID) Y");
            sb.Append(" ON RatingCatFeedback.RatingCatFeedbackID = Y.RatingCatFeedbackID");

            return sb.ToString();
        }
    }
}
