using System;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PPL.App
{
    public static class CekEnviVarutkDB
    {

        [FunctionName("CekEnviVarutkDB")]
        public static SqlDataReader Redirect(string query)
        {
            SqlDataReader res = null;
            try 
            {
                string username = Environment.GetEnvironmentVariable("DB_USERNAME");
                string password = Environment.GetEnvironmentVariable("DB_PASSWORD");
                string serverName = Environment.GetEnvironmentVariable("DB_NAMASERVER");
                string database = Environment.GetEnvironmentVariable("DB_ECOM");

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = serverName+".database.windows.net"; 
                builder.UserID = username;            
                builder.Password = password;
                builder.InitialCatalog = database;

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();       
                StringBuilder sb = new StringBuilder();
                sb.Append(query);
                String sql = sb.ToString();

                SqlCommand command = new SqlCommand(sql, connection);
                
                SqlDataReader reader = command.ExecuteReader();
                
                res = reader;
                
                return res;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return res;
        }

        [FunctionName("ChangeDateFormat")]
        public static string changeDate(string date) {

            string dateConvert = "";
            string tempDate = date;
            string[] arrDate = tempDate.Split("/");
            Dictionary<string, string> listIndoMonth = new Dictionary<string, string>();
            listIndoMonth.Add("1", "January"); listIndoMonth.Add("01", "January");
            listIndoMonth.Add("2", "February"); listIndoMonth.Add("02", "February");
            listIndoMonth.Add("3", "March"); listIndoMonth.Add("03", "March");
            listIndoMonth.Add("4", "April"); listIndoMonth.Add("04", "April");
            listIndoMonth.Add("5", "May"); listIndoMonth.Add("05", "May");
            listIndoMonth.Add("6", "June"); listIndoMonth.Add("06", "June");
            listIndoMonth.Add("7", "July"); listIndoMonth.Add("07", "July");
            listIndoMonth.Add("8", "August"); listIndoMonth.Add("08", "August");
            listIndoMonth.Add("9", "September"); listIndoMonth.Add("09", "September");
            listIndoMonth.Add("10", "October");
            listIndoMonth.Add("11", "November");
            listIndoMonth.Add("12", "December");
            string monthIndoVersion = listIndoMonth[arrDate[0]]; 
            dateConvert = arrDate[1]+" "+monthIndoVersion+" "+arrDate[2];

            return dateConvert;

        }
    }
}
