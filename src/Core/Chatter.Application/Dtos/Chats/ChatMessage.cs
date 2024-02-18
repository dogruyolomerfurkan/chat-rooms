using Chatter.Application.Dtos.Users;

namespace Chatter.Application.Dtos.Chats;

public class ChatMessage
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
    /// Mesajın gönderilme tarihi
    /// </summary>
    public DateTime SentDate { get; set; }

    public UserShortInfoDto UserInfo { get; set; }
}