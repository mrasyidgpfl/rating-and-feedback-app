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
    public class HomePage {        

        public static Func<IHomePageTool> Factory  = () => new PPL.App.HomePageTool();

        [FunctionName("HomePage")]
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
            IHomePageTool staticObject = Factory.Invoke();
            string personId = req.Query["PersonId"];
            if (! staticObject.securityCheck(personId)){
                return new OkObjectResult($"Bad Input");
            }
            List<HomePageData> data = staticObject.GetYourProject(personId);
    
            foreach(HomePageData item in data){
                if(staticObject.IsProjectCompleted(item.projectId, personId)){
                    item.yourRating = staticObject.GetYourRating(item.projectId, personId);
                    item.customerRating = staticObject.GetCustomerRating(item.projectId);
                }
            }

            string json = JsonConvert.SerializeObject(data);
            return new OkObjectResult(json);
        }
    }

}
