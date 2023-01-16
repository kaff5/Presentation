using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Presentation1.Models
{
    public class BookObjectId
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } = 100;
        public string Type { get; set; } = "BookObjectId";
    }
}
