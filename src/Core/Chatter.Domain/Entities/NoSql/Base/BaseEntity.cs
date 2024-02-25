using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chatter.Domain.Entities.NoSql.Base;

public abstract class BaseEntity
{
    // = ObjectId.GenerateNewId().ToString();
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } 

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ModifiedDate { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? DeletedDate { get; set; }

    public bool IsDeleted { get; set; }
}