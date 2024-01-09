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

    public interface IReportChartTool{
        List<string> GetProjectRatingPeriod(string projectId);
        List<float> TabulateAllSprint(string projectId);
        float TabulateEachSprint(string ratingPeriodId);
    }
    

    public class ReportChartTool : IReportChartTool{
        public static Func<ISQLEmulationClass> Factory  = () => new PPL.App.SQLEmulationClass();

        public List<string> GetProjectRatingPeriod(string projectId){
            StringBuilder sb = new StringBuilder("SELECT RatingPeriodID ", 350);
            sb.Append("FROM ProjectRatingPeriod JOIN ProjectRating ");
            sb.Append("ON ProjectRatingPeriod.ProjectID = ProjectRating.ProjectID ");
            sb.Append("WHERE ProjectRatingPeriod.ProjectID = '");
            sb.Append(projectId);
            sb.Append("'ORDER BY PeriodNumber;");
            string query = sb.ToString();
            List<string> result = new List<string>();
            List<List<string>> hasil = Factory.Invoke().ProcessSql(sb.ToString());
            foreach(List<string> item in hasil){
                result.Add(item[0]);
            }

            return result;
        }

        public List<float> TabulateAllSprint(string projectId){
            List<string> projectRatingPeriodList = GetProjectRatingPeriod(projectId);
            List<float> result = new List<float>();
            foreach(string ratingPeriod in projectRatingPeriodList){
                result.Add(TabulateEachSprint(ratingPeriod));
            }
            return result;
        }


        public float TabulateEachSprint(string ratingPeriodId){
            StringBuilder sb = new StringBuilder("SELECT RatingScale", 500);
            sb.Append(" FROM Rating ");
            sb.Append("WHERE RatingPeriodID = '");
            sb.Append(ratingPeriodId);
            sb.Append("';");
            string query = sb.ToString();
            List<List<string>> hasil = Factory.Invoke().ProcessSql(sb.ToString());
            float total = 0;
            float count = 0;

            foreach(List<string> item in hasil){
                int rating = Int32.Parse(item[0]);
                if(rating > 0){
                    total += rating;
                    count += 1;
                }
            }
            if(count == 0){
                return 0;
            }
            return total / count;
        }
    }
}