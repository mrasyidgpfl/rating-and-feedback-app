using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace PPL.App
{
    public static class SendEmail
    {
        [FunctionName("SendEmail")]
        public static async Task Send(
            [TimerTrigger("0 15 * * 5")]TimerInfo myTimer, ILogger log)
        {
            
            dynamic client = SendGridAPI();
            List<string> temp = getAll();
            string link = "";
            if (temp.Count > 0) {
                foreach (string iter in temp) {
                    string[] parserData = iter.Split("#");
                    link = "https://ecomfeedback.z23.web.core.windows.net/#/giverating/"+parserData[3];
                    SendAsync(client, parserData[4], parserData[1], parserData[2], parserData[0], link).Wait();
                }
            }
        }

        private static dynamic SendGridAPI(){
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API",
                                        EnvironmentVariableTarget.Machine);
            return new SendGridClient(apiKey);
        }


        private static async Task SendAsync(dynamic client, string to, string custName, string projectName, string perName, string link)
        {
            var msg = new SendGridMessage(){
                From = new EmailAddress("admin@ecomindo.com",
                                        "Ecomindo Customer Rating"),
                Subject = "Ecomindo Customer Rating",
                HtmlContent = "Ecomindo Kece",
                TemplateId = Environment.GetEnvironmentVariable("TEMPLATEGIVERATING_ID",
                                        EnvironmentVariableTarget.Machine)
            };

            msg.AddTo(new EmailAddress(to));
            msg.AddSubstitution("{{CustomerName}}", custName);
            msg.AddSubstitution("{{ProjectName}}", projectName);
            msg.AddSubstitution("{{PeriodName}}", perName);
            msg.AddSubstitution("{{link}}", link);

            var response = await client.SendEmailAsync(msg);
            
            return;
        }

        public static List<string> getAll() {

            StringBuilder sb = new StringBuilder("SELECT StartPeriod, EndPeriod, ClientName, ProjectName, ");
            sb.Append("Link, Email ");
            sb.Append("FROM Rating R, ProjectRatingPeriod PRP, Project P, Client C, LinkCustandProj L ");
            sb.Append("WHERE R.RatingPeriodID = PRP.RatingPeriodID AND RatingScale = 0 ");
            sb.Append("AND PRP.ProjectID =  P.ProjectID AND L.RatForCustID = R.RatingID  AND ");
            sb.Append("R.ClientID = C.ClientID AND PRP.EndPeriod <= '"+DateTime.Now+"'; "); 
            SqlDataReader reader = CekEnviVarutkDB.Redirect(sb.ToString());
            
            List<string> res = new List<string>();

            while(reader.Read()) {
                
                string start = reader.GetDateTime(0).ToShortDateString(); 
                string end = reader.GetDateTime(1).ToShortDateString();
                string period = CekEnviVarutkDB.changeDate(start)+" - "+CekEnviVarutkDB.changeDate(end);
                string result = period+"#"+reader.GetString(2)+"#"+reader.GetString(3)+"#"+
                                reader.GetString(4)+"#"+reader.GetString(5);
                res.Add(result);
            }

            return res;
    
        }

        [FunctionName("validateKode")]
        public static string validate([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log) {
            Dictionary<string, string> temp = new Dictionary<string, string>();

            string kode = req.Query["kode"];

            StringBuilder sb = new StringBuilder("SELECT * ");
            sb.Append("FROM LinkCustandProj; "); 
            SqlDataReader reader = CekEnviVarutkDB.Redirect(sb.ToString());

            while (reader.Read()) {
                string ratID = reader.GetString(2);
                temp.Add(reader.GetString(0), ratID);
            }

            if (temp.ContainsKey(kode)) {
                return temp[kode];
            }
            else {
                return "The given code of link project is not valid";
            }
        }
    }
}
