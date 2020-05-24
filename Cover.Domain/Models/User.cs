using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Cover.Domain.Models
{
    public class User : IModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }


        [BsonElement("surname")]
        public string Surname { get; set; }


        [BsonElement("username")]
        public string Login { get; set; }


        [BsonElement("password")]
        public string Password { get; set; }


        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("userPosts")]
        public IEnumerable<string> PostIds { get; set; }

        [BsonElement("friends")]
        public IEnumerable<Friend> Friends { get; set; }

    }
}
