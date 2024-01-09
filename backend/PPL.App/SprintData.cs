using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic; 
using System.Text;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace PPL.App{
   public class SprintData{
        public double expectedNumberOfRating;
        public double numberOfRating;
        public string sprintStart;
        public string sprintEnd;
        public int totalRating;
        public double averageRating; 
        public Dictionary<string,List<string>> commentaryList;

        public SprintData(string sprintStart, string sprintEnd){
            this.sprintStart = sprintStart;
            this.sprintEnd = sprintEnd;
            this.expectedNumberOfRating = 0;
            this.numberOfRating = 0;
            this.totalRating = 0;
            this.averageRating = 0;
            this.commentaryList = new Dictionary<string,List<string>>();
        }

        public void AddCriticism(string category, string comment){
            if(!this.commentaryList.ContainsKey(category)){
                this.commentaryList.Add(category, new List<string>());
            }
            this.commentaryList[category].Add(comment);
        }

        public void RegisterRating(int rating){
            this.totalRating += rating;
            this.numberOfRating += 1;
            this.averageRating = this.totalRating / this.numberOfRating;
        }

    }
}
