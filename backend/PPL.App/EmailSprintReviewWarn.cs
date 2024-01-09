using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

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
    public class EmailSprintReviewWarn{
        public static Func<ISQLEmulationClass> EmailWarnFactory  = () => new PPL.App.SQLEmulationClass();


        public static IDictionary<string, List<string>> GetListOfPeopleToEmail(){
            List<List<string>> hasil = EmailWarnFactory.Invoke().ProcessSql(GetQuery());
            HashSet<string> collection = new HashSet<string>();
            foreach(List<string> item in hasil){
                 int daysSince = HowManyDaysSince(item[2]);
                 if(daysSince > 1 && daysSince % 2 == 0){
                     collection.Add(item[0]+"--"+item[1]);
                 }
            }

            IDictionary<string, List<string>> result = new Dictionary<string, List<string>> ();
            
            foreach(String item in collection){
                string[] splitResult = item.Split("--");
                string projectId = splitResult[0];
                string personID = splitResult[1];
                if (! result.ContainsKey(personID)){
                    result.Add(personID, new List<string>());
                }
                result[personID].Add(projectId);
            }
            return result;
        }

        public static int HowManyDaysSince(string dateString){
            Console.WriteLine(dateString);
            string cleanedString = dateString.Split(" ")[0];
            Console.WriteLine(cleanedString);
            string[] stringDate = cleanedString.Split("/");
            DateTime startDate = new DateTime(Int32.Parse(stringDate[2]), 
                        Int32.Parse(stringDate[0]), 
                        Int32.Parse(stringDate[1]));
            return (DateTime.Now - startDate).Days;
        }

        public static string GetQuery(){
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT PR.ProjectID, Assignor, CreatedDate FROM ");
            sb.Append("ProjectRating PR ");
            sb.Append("JOIN ");
            sb.Append("(SELECT ProjectID, Assignor, CreatedDate FROM ProjectRatingPeriod PRP ");
            sb.Append("JOIN ");
            sb.Append("(SELECT RatingPeriodID, Assignor, CreatedDate FROM Rating ");
            sb.Append("WHERE RatingScale = 0 AND Assignee != '' ) K ");
            sb.Append("ON PRP.RatingPeriodID = K.RatingPeriodID) T ");
            sb.Append("ON PR.ProjectID = T.ProjectID;");
            return sb.ToString();
        }

        [FunctionName("EmailWarn")]
        public static void Run([TimerTrigger("0 31 20 * * *")]TimerInfo myTimer, ILogger log){   
        //private static void Main(){
            IDictionary<string, List<string>> result = GetListOfPeopleToEmail();
            dynamic client = SendGridAPI();
            foreach(KeyValuePair<string, List<string>> item in result){
                string nama = GetName(item.Key);
                string email = GetEmail(item.Key);
                List<string> projectIds = item.Value;
                List<string> projectName = new List<string>();
                foreach(string projectId in projectIds){
                    projectName.Add(GetProjectName(projectId));
                }
                SendAsync(client, email, nama, projectName).Wait();
                Console.WriteLine("Kekirim");
            }
        }

        public static string GetName(string PersonID){
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT PersonName FROM ");
            sb.Append("Person ");
            sb.Append("WHERE PersonID ='");
            sb.Append(PersonID);
            sb.Append("';");
            String sql = sb.ToString();
            List<List<string>> hasil = EmailWarnFactory.Invoke().ProcessSql(sql);
            return hasil[0][0];
        }

        public static string GetEmail(string PersonID){
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT Email FROM ");
            sb.Append("Person ");
            sb.Append("WHERE PersonID ='");
            sb.Append(PersonID);
            sb.Append("';");
            String sql = sb.ToString();
            List<List<string>> hasil = EmailWarnFactory.Invoke().ProcessSql(sql);
            return hasil[0][0];
        }

        public static string GetProjectName(string ProjectID){
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT ProjectName FROM ");
            sb.Append("Project ");
            sb.Append("WHERE ProjectID ='");
            sb.Append(ProjectID);
            sb.Append("';");
            string sql = sb.ToString();
            List<List<string>> hasil = EmailWarnFactory.Invoke().ProcessSql(sql);
            return hasil[0][0];
        }

        private static dynamic SendGridAPI(){
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API",
                                        EnvironmentVariableTarget.Machine);
            return new SendGridClient(apiKey);
        }


        public static string ConvertProjectListToString(List<string> projectNames){
            StringBuilder sb = new StringBuilder("", 350);
            foreach(string projectName in projectNames){
                sb.Append(projectName);
            }
            return sb.ToString();
        }

    

        private static async Task SendAsync(dynamic client, string to, string fullName, List<string> projectNames)
        {

            var msg = new SendGridMessage(){
                From = new EmailAddress("admin@ecomindo.com",
                                        "Ecomindo misses you"),
                Subject = "Ecomindo misses you",
                HtmlContent = "Ecomindo Kece",
                TemplateId = Environment.GetEnvironmentVariable("TEMPLATEREMINDER_ID",
                                        EnvironmentVariableTarget.Machine)
            };

            

            msg.AddTo(new EmailAddress(to));
            msg.AddSubstitution("-FullName-", fullName);
            msg.AddSubstitution("-Project-", ConvertProjectListToString(projectNames));

            Console.WriteLine(to);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Body.ReadAsStringAsync().Result); // The message will be here
            Console.WriteLine(response.Headers.ToString());
        }
    }
}