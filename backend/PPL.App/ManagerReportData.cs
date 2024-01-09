using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic; 
using System.Text;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace PPL.App{
   public class ManagerReportData{
        public Dictionary<string, double> ratingList;
        public Dictionary<string, int> criticismCategoryCount;
        public Dictionary<string, string> pidDictionary;
        public Dictionary<string, string> pid;

        public ManagerReportData(){
            this.ratingList = new Dictionary<string, double>();
            this.criticismCategoryCount = new Dictionary<string, int>();
            this.pidDictionary = new Dictionary<string, string>();
        }

        public void RegisterEmployee(string pid, string name, double rating){
            this.pidDictionary[name] = pid;
            this.ratingList[name] = rating;
        }

        public void RegisterCategoryCriticism(string category)
        {
            if (!this.criticismCategoryCount.ContainsKey(category))
            {
                this.criticismCategoryCount.Add(category, 0);
            }
            this.criticismCategoryCount[category] += 1;
        }
    }
}
