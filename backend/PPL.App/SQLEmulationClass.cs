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

    public interface ISQLEmulationClass
    {
        List<List<string>> ProcessSql(string query);
    }

    public class SQLEmulationClass : ISQLEmulationClass
    {
        public List<List<string>> ProcessSql(string query)
        {
            Console.WriteLine(query);
            SqlDataReader reader = CekEnviVarutkDB.Redirect(query);
            List<List<string>> sqlResult = new List<List<string>>();
            while (reader.Read())
            {
                int length = reader.FieldCount;
                List<string> entry = new List<string>();
                for (int I = 0; I < length; I = I + 1)
                {
                    entry.Add(reader.GetValue(I).ToString());
                }
                sqlResult.Add(entry);
            }
            Console.WriteLine("~~~");
            return sqlResult;
        }
    }
}
