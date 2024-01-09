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
    public class WeeklyReport{
        public static Func<ISQLEmulationClass> ReportFactory  = () => new PPL.App.SQLEmulationClass();


        public static String Tabulate(){
            IDictionary<String, WeeklyReportObject> tabulationResult = new Dictionary<String, WeeklyReportObject>();
            Dictionary<String, String> projectIdToNameMapping = new Dictionary<String, String>();            
            List<List<String>> result = ReportFactory.Invoke().ProcessSql(GetUnfilledQuery());

            foreach(List<string> item in result){
                if(! tabulationResult.ContainsKey(item[1])){
                    tabulationResult.Add(item[1], new WeeklyReportObject(GetName(item[1])));
                }

                if(! projectIdToNameMapping.ContainsKey(item[0])){
                    projectIdToNameMapping.Add(item[0], GetProjectName(item[0]));
                }

                tabulationResult[item[1]].RegisterMissingFeedback(item[0], item[3]);
            }
            
            result = ReportFactory.Invoke().ProcessSql(GetFilledQuery());
            foreach(List<string> item in result){
                if(! tabulationResult.ContainsKey(item[1])){
                    tabulationResult.Add(item[1], new WeeklyReportObject(GetName(item[1])));
                }

                if(! projectIdToNameMapping.ContainsKey(item[0])){
                    projectIdToNameMapping.Add(item[0], GetProjectName(item[0]));
                }

                tabulationResult[item[1]].RegisterFilledFeedback(item[0], item[2]);
 
            }
            StringBuilder sb = new StringBuilder(""); 
            foreach(String item in projectIdToNameMapping.Keys){
                sb.Append("<p>");
                sb.Append(projectIdToNameMapping[item]);
                sb.Append("</p>");
                sb.Append(CreateReport(tabulationResult, item));
            }
            return sb.ToString();
        }

        public static String CreateReport(IDictionary<String, WeeklyReportObject> tabulationResult,
            String projectId){

            List<List<String>> result = new List<List<String>>();

            StringBuilder sb = new StringBuilder("<table>");
            sb.Append("<tr><td>Nama</td><td>Status Feedback</td><td>Last Activity</td></tr>");
            
            foreach(WeeklyReportObject person in tabulationResult.Values){
                Console.WriteLine(person.GetName());
                if(person.WorkerWorkInProject(projectId)){
                    int missingData;
                    try{
                        missingData = person.GetNumberOfMissingFeedbackData(projectId);
                    }
                    catch(Exception e){
                        missingData = 0;
                    }
                    int missingPeriod;
                    try{
                        missingPeriod = person.GetNumberOfMissingPeriodData(projectId);
                    }
                    catch(Exception e){
                        missingPeriod = 0;
                    }
                    
                    DateTime lastActivity = person.GetLastActivity(projectId);
                    if(missingData != null & missingPeriod != null & lastActivity != null){
                        String reportInHTML = GenerateTextForReport(person.GetName(), missingData, missingPeriod,
                        lastActivity);
                        sb.Append(reportInHTML);
                    }
                }
            }

            sb.Append("</table>");
            return sb.ToString();
        }


        public static string GenerateTextForReport(string namaOrang, int missingData, int missingPeriod,
                    DateTime lastActivity){
                        StringBuilder sb = new StringBuilder("<tr><td>");
                        sb.Append(namaOrang);
                        sb.Append("</td><td>");
                        if(missingData == 0){
                            sb.Append("Semua feedback sudah di isi");
                        }
                        else{
                            sb.Append(missingData);
                            sb.Append(" feedback belum diisi dari ");
                            sb.Append(missingPeriod);
                            sb.Append(" period");
                        }
                        sb.Append("</td><td>");
                        sb.Append(lastActivity.ToString("dd MMMM yyyy"));
                        sb.Append("</td></tr>");
                        return sb.ToString();
                    }




        public static string GetUnfilledQuery(){
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT PR.ProjectID, Assignor, CreatedDate, RatingPeriodID FROM ");
            sb.Append("ProjectRating PR ");
            sb.Append("JOIN ");
            sb.Append("(SELECT ProjectID, Assignor, CreatedDate, PRP.RatingPeriodID FROM ProjectRatingPeriod PRP ");
            sb.Append("JOIN ");
            sb.Append("(SELECT RatingPeriodID, Assignor, CreatedDate FROM Rating ");
            sb.Append("WHERE RatingScale = 0 AND Assignee != '' ) K ");
            sb.Append("ON PRP.RatingPeriodID = K.RatingPeriodID) T ");
            sb.Append("ON PR.ProjectID = T.ProjectID;");
            return sb.ToString();
        }

        public static string GetFilledQuery(){
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT PR.ProjectID, Assignor, LastUpdatedDate FROM ");
            sb.Append("ProjectRating PR ");
            sb.Append("JOIN ");
            sb.Append("(SELECT ProjectID, Assignor, K.LastUpdatedDate FROM ProjectRatingPeriod PRP ");
            sb.Append("JOIN ");
            sb.Append("(SELECT RatingPeriodID, Assignor, LastUpdatedDate FROM Rating ");
            sb.Append("WHERE RatingScale > 0 AND Assignee != '' ) K ");
            sb.Append("ON PRP.RatingPeriodID = K.RatingPeriodID) T ");
            sb.Append("ON PR.ProjectID = T.ProjectID;");
            return sb.ToString();
        }

        [FunctionName("WeeklyEmail")]
        public static void Run([TimerTrigger("0 36 13 * * *")]TimerInfo myTimer, ILogger log){   
             
            dynamic client = SendGridAPI();

            SendAsync(client, "winston.chandra@ui.ac.id", "Winston", Tabulate()).Wait();

        }

        public static string GetName(string PersonID){
            StringBuilder sb = new StringBuilder("", 350);
            sb.Append("SELECT PersonName FROM ");
            sb.Append("Person ");
            sb.Append("WHERE PersonID ='");
            sb.Append(PersonID);
            sb.Append("';");
            String sql = sb.ToString();
            List<List<string>> hasil = ReportFactory.Invoke().ProcessSql(sql);
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
            List<List<string>> hasil = ReportFactory.Invoke().ProcessSql(sql);
            return hasil[0][0];
        }

        private static dynamic SendGridAPI(){
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API",
                                        EnvironmentVariableTarget.Machine);
            return new SendGridClient(apiKey);
        }    

        private static async Task SendAsync(dynamic client, string to, string fullName, string report)
        {

            var msg = new SendGridMessage(){
                From = new EmailAddress("admin@ecomindo.com",
                                        "Ecomindo"),
                Subject = "Ecomindo Weekly Report",
                HtmlContent = report,
                TemplateId = Environment.GetEnvironmentVariable("TEMPLATEWEEKLY_ID",
                                        EnvironmentVariableTarget.Machine)
            };

            msg.AddTo(new EmailAddress(to));
            msg.AddSubstitution("-FullName-", fullName);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Body.ReadAsStringAsync().Result); // The message will be here
            Console.WriteLine(response.Headers.ToString());
        }
    }
}