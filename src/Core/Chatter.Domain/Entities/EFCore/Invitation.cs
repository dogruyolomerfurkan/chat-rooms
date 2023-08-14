using Chatter.Domain.Entities.EFCore.Base;
using Chatter.Domain.Enums;
using Chatter.Identity.Entities;

namespace Chatter.Domain.Entities.EFCore;

public class Invitation : BaseEntity<int>
{
    /// <summary>
    /// Oda Id'si
    /// </summary>
    public int RoomId { get; set;}
    /// <summary>
    /// Gönderen kullanıcı Id'si
    /// </summary>
    public string SenderUserId { get; set; }
    /// <summary>
    /// Davet edilen kullanıcı Id'si
    /// </summary>
    public string InvitedUserId { get; set; }
    /// <summary>
    /// Davet statüsü
    /// </summary>
    public InvitationStatus Status { get; set; }
    
    public ChatterUser SenderUser { get; set; }
    public ChatterUser InvitedUser { get; set; }
}