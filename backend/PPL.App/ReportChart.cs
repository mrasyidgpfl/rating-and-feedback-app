using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic; 
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PPL.App{
    public class ReportChart {        

        public static Func<IReportChartTool> Factory  = () => new PPL.App.ReportChartTool();

        [FunctionName("ReportChart")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {   

            IReportChartTool staticObject = Factory.Invoke();

            string projectId = req.Query["ProjectId"];
            
            if (projectId == null){
                return new OkObjectResult($"Bad Input");
            }

            List<float> result = staticObject.TabulateAllSprint(projectId);

            JArray data = new JArray();
            foreach(float number in result){
                data.Add(number);
            }

            return new OkObjectResult(data.ToString()); 
        }
    }

}
