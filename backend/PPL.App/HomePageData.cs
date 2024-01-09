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
    public class HomePageData
    {
        public string namaProject;
        public string namaClient;
        public string projectId;
        public string startDate;
        public string endDate;
        public float customerRating;
        public float yourRating;
        public bool active;

        public HomePageData(string projectId, string namaProject, string namaClient,
        string startDate, string endDate, string active)
        {
            this.projectId = projectId;
            this.namaProject = namaProject;
            this.namaClient = namaClient;
            this.startDate = startDate;
            this.endDate = endDate;
            this.customerRating = 0;
            this.yourRating = 0;
            if (active == "true")
            {
                this.active = true;
            }
            else
            {
                this.active = false;
            }
        }

    }

}
