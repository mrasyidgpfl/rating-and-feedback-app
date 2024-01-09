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

    public interface IResetDatabaseTool
    {
        string ResetBadge();
        string ResetRating();
        void Reset();
    }


    public class ResetDatabaseTool : IResetDatabaseTool
    {
        public static Func<ISQLEmulationClass> SprintFactory = () => new PPL.App.SQLEmulationClass();


        public string ResetRating()
        {
            return "UPDATE Rating SET RatingScale = 0, Comment = '';";
        }
        
        public string ResetBadge()
        {
            return "DELETE FROM RatingFeedback;";
        }

        public void Reset()
        {
            SprintFactory.Invoke().ProcessSql(ResetBadge());
            SprintFactory.Invoke().ProcessSql(ResetRating());
        }
    }
}
