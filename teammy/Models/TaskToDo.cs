using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace teammy.Models
{
    public class TaskToDo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("taskId")]
        public int TaskId { get; set; }

        [BsonElement("projectId")]
        public int ProjectId { get; set; }

        [BsonElement("teamId")]
        public int TeamId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("priority")]
        public string Priority { get; set; }

        [BsonElement("progress")]
        public string Progress { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("dueDate")]
        public DateTime DueDate { get; set; }

        [BsonElement("assignees")]
        public List<User> Assignees { get; set; }
    }
}
