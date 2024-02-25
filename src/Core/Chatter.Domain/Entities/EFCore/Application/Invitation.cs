using Chatter.Domain.Entities.EFCore.Application.Base;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Enums;

namespace Chatter.Domain.Entities.EFCore.Application;

public class Invitation : BaseEntity<int>
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
    /// <summary>
    /// Davet statüsü
    /// </summary>
    public InvitationStatus Status { get; set; }

    public Room Room { get; set; }
    public ChatterUser SenderUser { get; set; }
    public ChatterUser InvitedUser { get; set; }
}