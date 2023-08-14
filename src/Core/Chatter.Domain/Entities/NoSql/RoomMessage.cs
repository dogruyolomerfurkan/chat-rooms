using Chatter.Domain.Entities.NoSql.Base;
using MongoDB.Bson.Serialization.Attributes;

namespace Chatter.Domain.Entities.NoSql;

[BsonIgnoreExtraElements]
public class RoomMessage : BaseEntity
{
    /// <summary>
    /// Oda Id'si
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gönderen kullanıcı Id'si
    /// </summary>
    public string SenderUserId { get; set; }

    /// <summary>
    /// Mesaj içeriği
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gönderilme tarihi
    /// </summary>
    public DateTime SentDate { get; set; }

    /// <summary>
    /// Düzenlendi mi?
    /// </summary>
    public bool IsEdited { get; set; }
}