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
    public class WeeklyReportObject{

        private IDictionary<String, List<String>> missingFeedback;
        private IDictionary<String, DateTime> lastFeedback;
        private String name;

        public WeeklyReportObject(String name){
            this.name = name;
            this.lastFeedback = new Dictionary<String, DateTime>();
            this.missingFeedback = new Dictionary<String, List<String>>();
        }

        public void RegisterMissingFeedback(String projectId, String periodId){
            if(! this.missingFeedback.ContainsKey(projectId)){
                this.missingFeedback.Add(projectId, new List<String>());
            }
            this.missingFeedback[projectId].Add(periodId);
        }

        public void RegisterFilledFeedback(String projectId, String stringDate){
            DateTime objek = ConvertStringDate(stringDate);
            Console.WriteLine(projectId);
            if(! this.lastFeedback.ContainsKey(projectId)){
                this.lastFeedback.Add(projectId, objek);
            }
            else{
                if (objek > this.lastFeedback[projectId]){
                    this.lastFeedback[projectId] = objek;
                }
            }
        }

        public DateTime ConvertStringDate(String stringDate){
            string cleanedString = stringDate.Split(" ")[0];
            string[] splitStringDate = cleanedString.Split("/");
            DateTime dateObject = new DateTime(Int32.Parse(splitStringDate[2]), 
                        Int32.Parse(splitStringDate[0]), 
                        Int32.Parse(splitStringDate[1]));
            return dateObject;
        }

        public bool WorkerWorkInProject(string projectId){
            return this.missingFeedback.ContainsKey(projectId) || this.lastFeedback.ContainsKey(projectId);
        }

        public int GetNumberOfMissingFeedbackData(string projectId){
            return this.missingFeedback[projectId].Count;
        }

        public int GetNumberOfMissingPeriodData(string projectId){
            HashSet<String> hasil = new HashSet<String>();
            foreach(string periodID in this.missingFeedback[projectId]){
                hasil.Add(periodID);
            }
            return hasil.Count;
        }

        public DateTime GetLastActivity(string projectId){
            return this.lastFeedback[projectId];
        }

        public String GetName(){
            return this.name;
        }
    }
}