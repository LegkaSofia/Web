using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cover.Domain.Models
{
    public class Friend
    {
        [BsonElement("friendId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string friendId { get; set; }

        [BsonElement("friendName")]
        public string FriendName { get; set; }

        [BsonElement("friendSurname")]
        public string FriendSurname { get; set; }

        [BsonElement("friendUsername")]
        public string FriendLogin { get; set; }
    }
}
