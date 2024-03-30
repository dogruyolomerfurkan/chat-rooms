using Chatter.Application.Dtos.Invitations;
using Chatter.Application.Dtos.Rooms;
using Chatter.Application.Services.Chats;
using Chatter.Common.Exceptions;
using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Enums;
using Chatter.Persistence.RepositoryManagement.EfCore.Invitations;
using Chatter.Persistence.RepositoryManagement.EfCore.Rooms;
using Chatter.Persistence.RepositoryManagement.EfCore.Users;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Application.Services.Invites;

public class InvitationService : BaseService, IInvitationService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IInvitationRepository _invitationRepository;
    private new readonly UserManager<ChatterUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IChatService _chatService;

    public InvitationService(UserManager<ChatterUser> userManager, IChatService chatService, IUserRepository userRepository,
        IInvitationRepository invitationRepository, IRoomRepository roomRepository) : base(userManager)
    {
        _userManager = userManager;
        _chatService = chatService;
        _userRepository = userRepository;
        _invitationRepository = invitationRepository;
        _roomRepository = roomRepository;
    }

    public async Task InviteUserToRoomAsync(InviteUserToRoomInput inviteUserToRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomPermissions)
            .Include(x => x.RoomChatterUsers)
            .FirstOrDefaultAsync(x => x.Id == inviteUserToRoomInput.RoomId);

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var requesterUser = await _userManager.FindByIdAsync(inviteUserToRoomInput.RequestedUserId);
        if (requesterUser is null)
            throw new FriendlyException("İstek atan kullanıcı bulunamadı");

        if (!(IsFullAdmin(inviteUserToRoomInput.RequestedUserId) || room.RoomPermissions
                    .FirstOrDefault(x => x.ChatterUserId == inviteUserToRoomInput.RequestedUserId)?.PermissionType ==
                ChatPermissionType.Admin))
            throw new FriendlyException("Davet etme yetkiniz yok");

        if (room.Capacity <= room.RoomChatterUsers.Count)
            throw new FriendlyException("Chat kapasitesi dolu. Lütfen oda kapasitesini arttırın.");

        var invitedRoomChatterUser = await _userManager.FindByIdAsync(inviteUserToRoomInput.ChatterUserId);
        if (invitedRoomChatterUser is null)
            throw new FriendlyException("İşlem yapılmak istenilen kullanıcı bulunamadı");

        var existPermission =
            room.RoomPermissions.FirstOrDefault(x => x.ChatterUserId == inviteUserToRoomInput.ChatterUserId);

        if (existPermission is not null)
            throw new FriendlyException("İşlem yapılmak istenilen kullanıcı zaten chatte.");

        var newInvite = new Invitation()
        {
            RoomId = room.Id,
            SenderUserId = inviteUserToRoomInput.RequestedUserId,
            InvitedUserId = inviteUserToRoomInput.ChatterUserId,
            Status = InvitationStatus.Pending
        };

        await _invitationRepository.CreateAsync(newInvite);
    }

    public async Task AcceptInviteAsync(AcceptInvitationInput acceptInvitationInput)
    {
        var invitation = await _invitationRepository.Query()
            .FirstOrDefaultAsync(x => x.Id == acceptInvitationInput.InvitationId);

        if (invitation is null)
            throw new FriendlyException("Davet bulunamadı");

        if (invitation.InvitedUserId != acceptInvitationInput.ChatterUserId)
            throw new FriendlyException("Davet edilen kullanıcı ile işlem yapan kullanıcı aynı olmalıdır");

        if (invitation.Status != InvitationStatus.Pending)
            throw new FriendlyException("Davet durumu geçersiz");

        var room = await _roomRepository.FindAsync(invitation.RoomId);
        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        if (room.Capacity <= room.RoomChatterUsers.Count)
            throw new FriendlyException("Chat kapasitesi dolu. Odaya giriş yapamazsınız.");

        var invitedUser = await _userManager.FindByIdAsync(acceptInvitationInput.ChatterUserId);
        if (invitedUser is null)
            throw new FriendlyException("Davet edilen kullanıcı bulunamadı");

        var invitedUserRoom =
            room.RoomChatterUsers.FirstOrDefault(x => x.ChatterUserId == acceptInvitationInput.ChatterUserId);
        if (invitedUserRoom is not null)
            throw new FriendlyException("Davet edilen kullanıcı zaten chatte");

        var roomChatterUser = new RoomChatterUser()
        {
            ChatterUserId = acceptInvitationInput.ChatterUserId,
            RoomId = room.Id,
        };
        room.RoomChatterUsers?.Add(roomChatterUser);

        var roomPermission = new RoomPermission()
        {
            ChatterUserId = acceptInvitationInput.ChatterUserId,
            RoomId = room.Id,
            PermissionType = ChatPermissionType.Chatter
        };
        room.RoomPermissions?.Add(roomPermission);

        _roomRepository.Update(room);

        invitation.Status = InvitationStatus.Accepted;
        _invitationRepository.Update(invitation);
    }

    public async Task RejectInviteAsync(RejectInvitationInput rejectInvitationInput)
    {
        var invitation = await _invitationRepository.Query()
            .FirstOrDefaultAsync(x => x.Id == rejectInvitationInput.InvitationId);

        if (invitation is null)
            throw new FriendlyException("Davet bulunamadı");

        if (invitation.InvitedUserId != rejectInvitationInput.ChatterUserId)
            throw new FriendlyException("Davet edilen kullanıcı ile işlem yapan kullanıcı aynı olmalıdır");

        if (invitation.Status != InvitationStatus.Pending)
            throw new FriendlyException("Davet durumu geçersiz");

        var room = await _roomRepository.FindAsync(invitation.RoomId);
        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var invitedUser = await _userManager.FindByIdAsync(rejectInvitationInput.ChatterUserId);
        if (invitedUser is null)
            throw new FriendlyException("Davet edilen kullanıcı bulunamadı");

        invitation.Status = InvitationStatus.Rejected;
        _invitationRepository.Update(invitation);
    }
    
    public async Task<List<PendingInvitationDto>> GetPendingInvitationsAsync(string userId)
    {
        return _invitationRepository.Query()
            .Include(x => x.Room)
            .Include(x => x.SenderUser)
            .Where(x => x.InvitedUserId == userId && x.Status == InvitationStatus.Pending).Adapt<List<PendingInvitationDto>>();
    }
}