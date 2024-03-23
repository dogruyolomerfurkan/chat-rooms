using Chatter.Application.Dtos.Rooms;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Application.Dtos.Invitations;

public class PendingInvitationDto
{
    
    public int Id { get; set; }
    /// <summary>
    /// Oda Id'si
    /// </summary>
    public int RoomId { get; set;}

    /// <summary>
    /// Gönderen kullanıcı Id'si
    /// </summary>
    public string SenderUserId { get; set; } = null!;

    public RoomDto Room { get; set; }
    public ChatterUser SenderUser { get; set; }
}