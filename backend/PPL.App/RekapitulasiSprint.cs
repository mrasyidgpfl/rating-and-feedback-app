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

namespace PPL.App
{
    public class RekapitulasiSprint
    {

        public static Func<IRekapitulasiSprintTool> Factory = () => new PPL.App.RekapitulasiSprintTool();

        [FunctionName("RekapitulasiSprint")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            IRekapitulasiSprintTool staticObject = Factory.Invoke();

            string projectId = req.Query["ProjectId"];
            string personId = req.Query["PersonId"];

            if (projectId == null || personId == null)
            {
                return new OkObjectResult($"Bad Input");
            }

            List<SprintData> result = staticObject.TabulateEachSprint(projectId, personId);
            string json = JsonConvert.SerializeObject(result);

            return new OkObjectResult(json);

        }
    }

}
