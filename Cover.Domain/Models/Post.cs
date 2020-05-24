using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Cover.Domain.Models
{
    public class Post: IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("createdBy")]
        public string CreateBy { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [BsonElement("owner")]
        public string OwnerUserName { get; set; }

        [BsonElement("comments")]
        public List<Comment> Comments { get; set; }

        [BsonElement("likes")]
        public List<Like> Likes { get; set; }


    }
}
