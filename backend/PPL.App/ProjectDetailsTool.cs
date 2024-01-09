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

    public interface IProjectDetailsTool
    {
        string CustomerAverageRating(string projectId);
        string GetProjectName(string projectId);
        string GetClientName(string projectId);
    }


    public class ProjectDetailsTool : IProjectDetailsTool
    {
        public static Func<ISQLEmulationClass> SprintFactory = () => new PPL.App.SQLEmulationClass();


        public string GetProjectNameQuery(string projectId)
        {
            StringBuilder sb = new StringBuilder("SELECT ProjectName FROM Project ");
            sb.Append("WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("'");
            return sb.ToString();
        }

        public string CustomerAverageRating(string projectId)
        {
            Console.WriteLine("Rating");
            List<List<string>> result = SprintFactory.Invoke().ProcessSql(GetCostumerRatingQuery(projectId));
            int count = 0;
            int total = 0;
            foreach (List<string> row in result)
            {
                int rate = Int32.Parse(row[0]);
                if(rate > 0){
                    total += rate;
                    count += 1;
                }
            }
            Console.WriteLine("gnitaR");
            Console.WriteLine(total); Console.WriteLine(count);
            return ""+(total * 1.0 / count);
        }

        public string GetProjectName(string projectId)
        {
            Console.WriteLine("Project");
            List<List<string>> result = SprintFactory.Invoke().ProcessSql(GetProjectNameQuery(projectId));
            Console.WriteLine("tcejorP");
            return result[0][0];
        }

        public string GetClientName(string projectId)
        {
            Console.WriteLine("Client");
            List<List<string>> result = SprintFactory.Invoke().ProcessSql(GetClientNameQuery(projectId));
            Console.WriteLine(result[0][0]);
            return result[0][0]+"#"+result[0][1];
        }

        public string GetCostumerRatingQuery(string projectId)
        {
            StringBuilder sb = new StringBuilder("SELECT RatingScale FROM Rating R JOIN");
            sb.Append("(SELECT RatingPeriodID ");
            sb.Append("FROM ProjectRatingPeriod PRP WHERE ");
            sb.Append("ProjectID = '");
            sb.Append(projectId);
            sb.Append("') X ON R.RatingPeriodID = X.RatingPeriodID ");
            sb.Append("WHERE Assignee =''");
            return sb.ToString();
        }

        public string GetClientNameQuery(string projectId)
        {
            StringBuilder sb = new StringBuilder("SELECT ClientName, C.ClientID FROM Client C JOIN");
            sb.Append("(SELECT ClientID FROM Project ");
            sb.Append("WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("' ) X ON C.ClientID = X.ClientID ");
            return sb.ToString();
        }
    }
}
