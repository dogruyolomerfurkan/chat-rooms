namespace Chatter.Application.Dtos.Invitations;

public class RejectInvitationInput
{
    public int InvitationId { get; set; }
    public int RoomId { get; set; }
    public string ChatterUserId { get; set; } 
}