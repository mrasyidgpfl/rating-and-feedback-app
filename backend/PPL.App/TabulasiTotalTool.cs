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

namespace PPL.App
{

    public interface ITabulasiTotalTool
    {
        List<string> GetProjectRatingPeriod(string projectId);
        bool IsSprintCompleted(string ratingPeriodId, string personId);
        IDictionary<string, int> Tabulate(string ratingPeriodId, string personId);
        bool securityCheck(string projectId, string personId);
        string GetProjectName(string ProjectID);
    }


    public class TabulasiTotalTool : ITabulasiTotalTool
    {
        public static Func<ISQLEmulationClass> Factory = () => new PPL.App.SQLEmulationClass();

        public bool securityCheck(string projectID, string personId)
        {

            Regex r = new Regex("^[a-zA-Z0-9]*$");

            if (projectID == null)
            {
                return false;
            }
            if (personId == null)
            {
                return false;
            }
            if (projectID.Length == 0)
            {
                return false;
            }
            if (personId.Length == 0)
            {
                return false;
            }
            if (!r.IsMatch(projectID))
            {
                return false;
            }
            if (!r.IsMatch(personId))
            {
                return false;
            }
            return true;
        }

        public List<string> GetProjectRatingPeriod(string projectId)
        {
            StringBuilder sb = new StringBuilder("SELECT RatingPeriodID ", 350);
            sb.Append("FROM ProjectRatingPeriod JOIN ProjectRating ");
            sb.Append("ON ProjectRatingPeriod.ProjectID = ProjectRating.ProjectID ");
            sb.Append("WHERE ProjectRatingPeriod.ProjectID = '");
            sb.Append(projectId);
            sb.Append("';");
            string query = sb.ToString();
            List<string> result = new List<string>();
            List<List<string>> hasil = Factory.Invoke().ProcessSql(sb.ToString());
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
            List<List<string>> hasil = Factory.Invoke().ProcessSql(sb.ToString());
            foreach (List<string> item in hasil)
            {
                return false;
            }
            return true;
        }

        public string GetProjectName(string ProjectID)
        {
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT ProjectName FROM ");
            sb.Append("Project ");
            sb.Append("WHERE ProjectID ='");
            sb.Append(ProjectID);
            sb.Append("';");
            string sql = sb.ToString();
            List<List<string>> hasil = Factory.Invoke().ProcessSql(sql);
            return hasil[0][0];
        }

        public IDictionary<string, int> Tabulate(string ratingPeriodId, string personId)
        {
            StringBuilder sb = new StringBuilder("SELECT RatingScale, Assignee ", 500);
            sb.Append("FROM Rating ");
            sb.Append("WHERE RatingPeriodID = '");
            sb.Append(ratingPeriodId);
            sb.Append("';");
            string query = sb.ToString();
            List<List<string>> hasil = Factory.Invoke().ProcessSql(query);
            Dictionary<string, int> tabulasi = new Dictionary<string, int>();
            tabulasi.Add("team total", 0);
            tabulasi.Add("team count", 0);
            tabulasi.Add("personal total", 0);
            tabulasi.Add("personal count", 0);

            foreach (List<string> item in hasil)
            {
                int nilai = Int32.Parse(item[0]);
                if (nilai > 0)
                {
                    tabulasi["team total"] += nilai;
                    tabulasi["team count"] += 1;
                    if (item[1] == personId)
                    {
                        tabulasi["personal total"] += nilai;
                        tabulasi["personal count"] += 1;
                    }
                }
            }
            return tabulasi;
        }
    }
}
