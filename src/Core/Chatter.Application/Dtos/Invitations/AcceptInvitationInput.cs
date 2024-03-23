namespace Chatter.Application.Dtos.Invitations;

public class AcceptInvitationInput
{
    public int InvitationId { get; set; }
    public int RoomId { get; set; }
    public string ChatterUserId { get; set; }
}