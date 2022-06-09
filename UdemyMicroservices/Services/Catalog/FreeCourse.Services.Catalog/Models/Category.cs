using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Models
{
    public class Category
    {
        [BsonId]//tip belittik
        [BsonRepresentation(BsonType.ObjectId)]//object ıd si olduğunu verdik mongo db ye
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
