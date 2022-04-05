using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teammy.Models.Entities
{
    public class Team
    {
        public ObjectId _id { get; set; }
        public int teamId { get; set; }
        public string teamName { get; set; }
        public BsonArray members { get; set; }

    }
}
