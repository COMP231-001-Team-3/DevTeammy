using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace teammy.Models
{
    public class Project
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("projectId")]
        public int ProjectId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("teamId")]
        public int TeamId { get; set; }

        [BsonElement("categories")]
        public List<string> Categories { get; set; }
    }
}
