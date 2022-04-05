using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teammy.Models.Entities
{
    public class DoTask
    {
        public ObjectId _id { get; set; }
        public int taskId { get; set; }
        public string title { get; set; }
        public int projectId { get; set; }
        public string priority { get; set; }
        public DateTime dueDate { get; set; }
        public string progress { get; set; }
        public string category { get; set; }
        public int teamId { get; set; }
        public User[] assignees { get; set; }

    }
}
