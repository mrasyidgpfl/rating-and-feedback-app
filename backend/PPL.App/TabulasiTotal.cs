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

namespace PPL.App{
    public class TabulasiTotal {        

        public static Func<ITabulasiTotalTool> Factory  = () => new PPL.App.TabulasiTotalTool();

        [FunctionName("TotalTabulation")]
        /**
        * @api {post} /Run
        *
        * @apiParam {String} PersonId  Id dari sang user
        * @apiParam {String} Project  Id dari sang project
        *
        * @apiSuccess {int} lastname  Lastname of the User.
        */
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {   

            ITabulasiTotalTool staticObject = Factory.Invoke();

            string projectId = req.Query["ProjectId"];
            string personId = req.Query["PersonId"];
            
            if (! staticObject.securityCheck(projectId, personId)){
                return new OkObjectResult($"Bad Input");
            }

            Dictionary<string, double> tabulasi = new Dictionary<string, double>();
            tabulasi.Add("personal count", 0);
            tabulasi.Add("personal total", 0);
            tabulasi.Add("team count", 0);
            tabulasi.Add("team total", 0);

            List<string> availablePeriod = staticObject.GetProjectRatingPeriod(projectId);
            
            foreach(string period in availablePeriod){
                if(staticObject.IsSprintCompleted(period, personId)){
                    IDictionary<string, int> sprintResult = staticObject.Tabulate(period, personId);
                    foreach(KeyValuePair<string, int> entry in sprintResult){
                        tabulasi[entry.Key] += entry.Value;
                    }
                }
            }

            JObject o = new JObject();
            
            if(tabulasi["personal count"] == 0){
                o["personalAverage"] = "nan";
            }
            else{
                o["personalAverage"] = tabulasi["personal total"] / tabulasi["personal count"];
            }
            if(tabulasi["team count"] == 0){
                o["teamAverage"] = "nan";
            }
            else{
                o["teamAverage"] = tabulasi["team total"] / tabulasi["team count"];
            }

            o["name"] = staticObject.GetProjectName(projectId);
            return new OkObjectResult(o.ToString()); 
        }
    }

}
