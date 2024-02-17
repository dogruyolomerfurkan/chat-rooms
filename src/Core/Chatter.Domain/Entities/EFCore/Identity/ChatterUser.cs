using Chatter.Domain.Entities.EFCore.Application;
using Microsoft.AspNetCore.Identity;

namespace Chatter.Domain.Entities.EFCore.Identity;

public class ChatterUser : IdentityUser
{
    public ChatterUser()
    {
        SentInvitations = new List<Invitation>();
        ReceivedInvitations = new List<Invitation>();
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? StatusDescription { get; set; }
    public string ProfileImagePath { get; set; } = null!;
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? ModifiedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public bool IsDeleted { get; set; }
    
    public List<Invitation>? SentInvitations { get; set; }
    public List<Invitation>? ReceivedInvitations { get; set; }
    public List<RoomChatterUser>? RoomChatterUsers { get; set; }

    public List<RoomChatterUser>? RoomBlockedChatterUser
    {
        get => RoomChatterUsers?.Where(x => x.IsBlocked).ToList();
    }
}
