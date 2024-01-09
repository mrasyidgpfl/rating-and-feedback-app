using System;
using System.Collections.Generic;
using System.IO; 
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PPL.App
{
    public class GetPersonandPeriod
    {
        [FunctionName("GetPersonbyPeriod")]
        public static Dictionary<string, List<string>> getPersonbyPeriodData([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Dictionary<string,string> person = new Dictionary<string,string>();
            Dictionary<string, List<string>> personandPeriod = new Dictionary<string,List<string>>();          
            string namaProyek = req.Query["proyekName"];

            StringBuilder sb1 = new StringBuilder("SELECT PersonID, PersonName ");
            sb1.Append("FROM Person; ");
            string queryGetPerson = sb1.ToString();
            SqlDataReader reader2 = CekEnviVarutkDB.Redirect(queryGetPerson);
            while (reader2.Read()) {
                string pid = reader2.GetString(0); string nama = reader2.GetString(1);
                person.Add(pid, nama);
            }

            StringBuilder sb = new StringBuilder("SELECT StartPeriod, EndPeriod, Assignee, RatingID ");
            sb.Append("FROM ProjectRatingPeriod, Rating ");
            sb.Append("WHERE ProjectRatingPeriod.ProjectID = '");
            sb.Append(namaProyek);
            sb.Append("' AND ProjectRatingPeriod.RatingPeriodID = Rating.RatingPeriodID AND RatingScale = 0 AND RatingCategoryID = 1; ");
            string queryGetPersonandProject = sb.ToString();
            SqlDataReader reader1 = CekEnviVarutkDB.Redirect(queryGetPersonandProject);
            while(reader1.Read()) {
                if (!(reader1.GetString(2).Length == 2)) {
                    string period = reader1.GetDateTime(0).ToShortDateString()+" - "+reader1.GetDateTime(1).ToShortDateString();
                    string namaOrang = person[reader1.GetString(2)]+","+reader1.GetString(2)+","+reader1.GetString(3);
                    if (!personandPeriod.ContainsKey(period)) {
                        List<string> arrayOfPerson = new List<string>();
                        arrayOfPerson.Add(namaOrang);
                        personandPeriod.Add(period, arrayOfPerson);
                    }
                    else {
                        Boolean cek = false;
                        foreach(string nama in personandPeriod[period]) {
                            string[] tempArr = nama.Split(",");
                            if (tempArr[0].Equals(person[reader1.GetString(2)])) {
                                cek = true; break;
                            }
                        }
                        if (cek == false) {
                            personandPeriod[period].Add(namaOrang);
                        }
                    }
                }
            }

            return personandPeriod;
        }
    }
}
