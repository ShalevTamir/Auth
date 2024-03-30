using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Mongo.Models
{
    public class User
    {
        public ObjectId _id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
