using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teammy.Models.Entities
{
    public class User
    {
        public ObjectId _id { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string privilege { get; set; }
        public string preferCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
