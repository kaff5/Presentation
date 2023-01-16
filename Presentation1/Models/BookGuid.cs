using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Presentation1.Models
{
    public class BookGuid
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } = 100;
        public string Type { get; set; } = "Guid";
    }
}
