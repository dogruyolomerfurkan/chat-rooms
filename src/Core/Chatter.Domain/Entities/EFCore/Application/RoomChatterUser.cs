using Chatter.Domain.Entities.EFCore.Application.Base;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Domain.Entities.EFCore.Application;

public class RoomChatterUser : BaseEntity<int>
{
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public string ChatterUserId { get; set; }
    public ChatterUser ChatterUser { get; set; }
    public bool IsBlocked { get; set; } = false;
}