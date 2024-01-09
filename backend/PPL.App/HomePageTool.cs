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
    
    public interface IHomePageTool{
        List<HomePageData> GetYourProject(string projectId);
        float GetYourRating(string projectId, string personId);
        bool IsProjectCompleted(string projectId, string personId);
        bool securityCheck(string personId);
        float GetCustomerRating(string projectId);
    }

    public class HomePageTool:IHomePageTool{
        public static Func<ISQLEmulationClass> HomePageFactory  = () => new PPL.App.SQLEmulationClass();

        public bool securityCheck(string personId){
           Regex r = new Regex("^[a-zA-Z0-9]*$");

            if(personId == null){
                return false;
            }
            if(personId.Length == 0){
                return false;
            }
            return r.IsMatch(personId);
        }

        public List<HomePageData> GetYourProject(string personId){
            StringBuilder sb = new StringBuilder("SELECT ClientName, ProjectID, ProjectName, StartPeriod, EndPeriod, Aktif ", 350);
            sb.Append("FROM Client ");
            sb.Append("JOIN ");
            sb.Append("(SELECT ClientID, Project.ProjectID, ProjectName, StartPeriod, EndPeriod, Aktif ");
            sb.Append("FROM Project JOIN ProjectAssignment ");
            sb.Append("ON Project.ProjectID = ProjectAssignment.ProjectID ");
            sb.Append("WHERE PersonId = '");
            sb.Append(personId);
            sb.Append("') X ON Client.ClientID = X.ClientID");
            List<List<string>> hasil = HomePageFactory.Invoke().ProcessSql(sb.ToString());
            int count = 0;
            List<HomePageData> daftarProject = new List<HomePageData>();
            foreach(List<string> item in hasil){
                HomePageData o = new HomePageData(item[1], item[2], item[0], item[3], item[4], item[5]);
                daftarProject.Add(o);
            }
            return daftarProject;
        }

        public float GetYourRating(string projectId, string personId){
            StringBuilder sb = new StringBuilder("SELECT RatingScale FROM ", 350);
            sb.Append("Rating JOIN ProjectRatingPeriod ");
            sb.Append("On Rating.RatingPeriodID = ProjectRatingPeriod.RatingPeriodId ");
            sb.Append("WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("' AND Assignee = '");
            sb.Append(personId);
            sb.Append("';");
            float total = 0;
            int count = 0;
            List<List<string>> hasil = HomePageFactory.Invoke().ProcessSql(sb.ToString());
            foreach(List<string> item in hasil){
                int rate = int.Parse(item[0]);
                if(rate > 0){
                    total += rate;
                    count += 1;
                }
            }
            if(count == 0){
                return 0;
            }
            return total / count;
        }

        public float GetCustomerRating(string projectId){
            StringBuilder sb = new StringBuilder("SELECT RatingScale FROM ", 350);
            sb.Append("Rating JOIN ProjectRatingPeriod ");
            sb.Append("On Rating.RatingPeriodID = ProjectRatingPeriod.RatingPeriodId ");
            sb.Append("WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("' AND Assignee = '';");
            float total = 0;
            int count = 0;
            List<List<string>> hasil = HomePageFactory.Invoke().ProcessSql(sb.ToString());
            foreach(List<string> item in hasil){
                int rate = int.Parse(item[0]);
                if(rate > 0){
                    total += rate;
                    count += 1;
                }
            }
            if(count == 0){
                return 0;
            }
            return total / count;
        }

        public bool IsProjectCompleted(string projectId, string personId){
            StringBuilder sb = new StringBuilder("SELECT * FROM ", 350);
            sb.Append("Rating JOIN ProjectRatingPeriod ");
            sb.Append("On Rating.RatingPeriodID = ProjectRatingPeriod.RatingPeriodId ");
            sb.Append("WHERE ProjectID = '");
            sb.Append(projectId);
            sb.Append("' AND Assignor = '");
            sb.Append(personId);
            sb.Append("'AND RatingScale = 0;");
            List<List<string>> hasil = HomePageFactory.Invoke().ProcessSql(sb.ToString());
            return hasil.Count == 0;
        } 
    }
}
