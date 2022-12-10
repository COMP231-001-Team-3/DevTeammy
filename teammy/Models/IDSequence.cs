using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace teammy.Models
{
    public class IDSequence
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("myID")]
        public string myID { get; set; }

        [BsonElement("projectsId")]
        public int ProjectId { get; set; }

        [BsonElement("teamsId")]
        public int TeamId { get; set; }

        [BsonElement("tasksId")]
        public int TaskId { get; set; }

        [BsonElement("usersId")]
        public int UserId { get; set; }
    }
}
