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
    public static class BadgeRecapitulation{
        
        public static Func<IBadgeRecapitulationTool> Factory  = () => new PPL.App.BadgeRecapitulationTool();
        
        [FunctionName("BadgeTabulation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {   
            string projectId = req.Query["ProjectId"];
            string personId = req.Query["PersonId"];

            IBadgeRecapitulationTool staticObject = Factory.Invoke();
            
            if (!staticObject.securityCheck(projectId, personId)){
                return new OkObjectResult($"Bad Input");
            }
            
            List<string> availableCategory = staticObject.GetFeedbackCategory(projectId);
            IDictionary<string, int> badgeCount = new Dictionary<string, int>();
            foreach(string category in availableCategory){
                badgeCount.Add(category, 0);
            }
            List<string> availablePeriod = staticObject.GetProjectRatingPeriod(projectId);
            foreach(string period in availablePeriod){
                if(staticObject.IsSprintCompleted(period, personId)){
                    IDictionary<string, int> sprintResult = staticObject.CollectBadge(period, personId);
                    foreach(KeyValuePair<string, int> entry in sprintResult){
                        badgeCount[entry.Key] += entry.Value;
                    }
                }
            }
            JArray data = new JArray();
            foreach(string category in availableCategory){
                JArray item = new JArray();
                item.Add(category);
                item.Add(badgeCount[category]);
                data.Add(item);
            }

            return new OkObjectResult(data.ToString()); 
        }
    }
}
