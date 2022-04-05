using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teammy.Models.Entities
{
    public class Project
    {
        public ObjectId _id { get; set; }
        public int projectId { get; set; }
        public string name { get; set; }
        public int teamId { get; set; }
        public string[] categories { get; set; }
    }
}
