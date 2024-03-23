using Chatter.Application.Dtos.Invitations;
using Chatter.Application.Dtos.Rooms;

namespace Chatter.Application.Services.Invites;

public interface IInvitationService
{
    /// <summary>
    /// Odaya kullanıcı davet eder
    /// </summary>
    /// <param name="inviteUserToRoomInput"></param>
    /// <returns></returns>
    Task InviteUserToRoomAsync(InviteUserToRoomInput inviteUserToRoomInput);

    /// <summary>
    /// Daveti kabul eder
    /// </summary>
    /// <param name="acceptInvitationInput"></param>
    /// <returns></returns>
    Task AcceptInviteAsync(AcceptInvitationInput acceptInvitationInput);

    /// <summary>
    /// Daveti reddeder
    /// </summary>
    /// <param name="rejectInvitationInput"></param>
    /// <returns></returns>
    Task RejectInviteAsync(RejectInvitationInput rejectInvitationInput);
    
    /// <summary>
    /// Bekleyen davetleri getirir
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<PendingInvitationDto>> GetPendingInvitationsAsync(string userId);

}