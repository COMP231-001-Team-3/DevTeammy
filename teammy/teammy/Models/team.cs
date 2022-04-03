using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace teammy.Models
{
    public class Team
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("teamId")]
        public int TeamId { get; set; }

        [BsonElement("teamName")]
        public string TeamName { get; set; }

        [BsonElement("members")]
        public List<User> Members { get; set; }
        
        [BsonElement("projects")]
        public List<int> Projects { get; set; }
    }
}
