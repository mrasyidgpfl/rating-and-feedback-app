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

namespace PPL.App
{
    public static class ProjectDetail
    {

        public static Func<IProjectDetailsTool> Factory = () => new PPL.App.ProjectDetailsTool();

        [FunctionName("ProjectDetails")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string projectId = req.Query["ProjectId"];

            IProjectDetailsTool staticObject = Factory.Invoke();

            JArray data = new JArray();
            string[] tempCust = staticObject.GetClientName(projectId).Split("#");
            data.Add(staticObject.GetProjectName(projectId));
            data.Add(tempCust[0]);
            data.Add(staticObject.CustomerAverageRating(projectId));
            data.Add(tempCust[1]);
            Console.WriteLine(data.ToString());
            return new OkObjectResult(data.ToString());
        }
    }
}
