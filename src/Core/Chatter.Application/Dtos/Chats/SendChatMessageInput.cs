namespace Chatter.Application.Dtos.Chats;

public class SendChatMessageInput
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


}