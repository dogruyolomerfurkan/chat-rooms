namespace Chatter.Application.Dtos.Invitations;

public class CreateInvitationInput
{
    /// <summary>
    /// Oda Id'si
    /// </summary>
    public int RoomId { get; set;}

    /// <summary>
    /// Gönderen kullanıcı Id'si
    /// </summary>
    public string SenderUserId { get; set; } = null!;

    /// <summary>
    /// Davet edilen kullanıcı Id'si
    /// </summary>
    public string InvitedUserId { get; set; } = null!;
   
}