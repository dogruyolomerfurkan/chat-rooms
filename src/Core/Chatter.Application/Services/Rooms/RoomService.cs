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

namespace Chatter.Application.Services.Rooms;

public class RoomService : BaseService, IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly UserManager<ChatterUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IChatService _chatService;

    public RoomService(IRoomRepository roomRepository,
        UserManager<ChatterUser> userManager, IUserRepository userRepository, IChatService chatService) : base(userManager)
    {
        _roomRepository = roomRepository;
        _userManager = userManager;
        _userRepository = userRepository;
        _chatService = chatService;
    }

    public async Task<List<RoomDto>> GetRoomsAsync()
    {
        var rooms = await _roomRepository.Query(true)
            .Include(x => x.RoomChatterUsers)
            .ToListAsync();

        return rooms.Adapt<List<RoomDto>>();
    }

    public async Task<List<RoomDto>> GetRoomsByUserIdAsync(string userId)
    {
        var roomIds = await _userRepository.Query(true)
            .Where(x => x.Id == userId)
            .Include(x => x.RoomChatterUsers)
            .SelectMany(x => x.RoomChatterUsers!).Select(x => x.RoomId).ToListAsync();

        var rooms = await _roomRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .Where(x => roomIds.Contains(x.Id)).ToListAsync();

        var roomDtos = rooms.Adapt<List<RoomDto>>();
        foreach (var room in roomDtos)
        {
            room.LastChatMessage = await _chatService.GetLastMessageAsync(room.Id);
        }

        return roomDtos.OrderByDescending(x => x.LastChatMessage != null ? x.LastChatMessage?.SentDate : x.CreatedDate)
            .ToList();
    }

    public async Task<List<RoomDto>> GetPublicRooms()
    {
        var rooms = await _roomRepository.Query(true)
            .Where(x => x.IsPublic)
            .Include(x => x.RoomChatterUsers).ToListAsync();

        return rooms.Adapt<List<RoomDto>>();
    }

    public async Task<RoomDto?> GetRoomDetailAsync(int roomId)
    {
        var room = await _roomRepository.Query(true)
            .Where(x => x.Id == roomId)
            .Include(x => x.RoomPermissions)
            .Include(x => x.Invitations.Where(inv => inv.Status == InvitationStatus.Pending))
            .ThenInclude(x => x.InvitedUser)
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
            .AsSplitQuery().FirstOrDefaultAsync();

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        return room.Adapt<RoomDto>(CreateTypeAdapterConfig(3));
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomInput createRoomInput)
    {
        var room = createRoomInput.Adapt<Room>();
        var roomPermission = new RoomPermission()
        {
            ChatterUserId = createRoomInput.Users.First().Id,
            RoomId = room.Id,
            PermissionType = ChatPermissionType.Admin,
        };
        room.RoomPermissions?.Add(roomPermission);
        var roomChatterUser = new RoomChatterUser()
        {
            ChatterUserId = createRoomInput.Users.First().Id,
            RoomId = room.Id,
        };
        room.RoomChatterUsers?.Add(roomChatterUser);
        await _roomRepository.CreateAsync(room);

        return room.Adapt<RoomDto>(CreateTypeAdapterConfig(3));
    }

    public Task BlockUserByRoomAsync(int roomId, ChatterUser blockedUser)
    {
        throw new NotImplementedException();
    }

    public async Task JoinRoomAsync(JoinRoomInput joinRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
            .FirstOrDefaultAsync(x => x.Id == joinRoomInput.RoomId);

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var user = await _userManager.FindByIdAsync(joinRoomInput.UserId);

        if (user is null)
            throw new FriendlyException("Kullanıcı bulunamadı");

        if (room.RoomChatterUsers.Select(x => x.ChatterUserId).Contains(user.Id))
            throw new FriendlyException("Kullanıcı zaten chatte");

        if (room.Capacity == room.RoomChatterUsers.Count)
            throw new FriendlyException("Chat kapasitesi dolu");

        var roomChatterUser = new RoomChatterUser()
        {
            ChatterUserId = user.Id,
            RoomId = room.Id,
        };
        room.RoomChatterUsers?.Add(roomChatterUser);

        var roomPermission = new RoomPermission()
        {
            ChatterUserId = user.Id,
            RoomId = room.Id,
            PermissionType = ChatPermissionType.Chatter
        };
        room.RoomPermissions?.Add(roomPermission);

        _roomRepository.Update(room);
    }

    public async Task LeaveRoomAsync(LeaveRoomInput leaveRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomPermissions)
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
            .FirstOrDefaultAsync(x => x.Id == leaveRoomInput.RoomId);

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var user = await _userManager.FindByIdAsync(leaveRoomInput.UserId);
        if (user is null)
            throw new FriendlyException("Kullanıcı bulunamadı");

        if (!room.RoomChatterUsers.Select(x => x.ChatterUserId).Contains(user.Id))
            throw new FriendlyException("Kullanıcı zaten chatte değil");

        if (room.RoomPermissions.FirstOrDefault(x => x.ChatterUserId == user.Id)?.PermissionType ==
            ChatPermissionType.Admin &&
            room.RoomPermissions.Count(x => x.PermissionType == ChatPermissionType.Admin) == 1)
            throw new FriendlyException("Chatte başka admin yok. Lütfen başka bir admin atayın.");

        var roomChatterUser = room.RoomChatterUsers.First(x => x.ChatterUserId == user.Id);
        roomChatterUser.IsDeleted = true;
        room.RoomPermissions?.Remove(room.RoomPermissions.First(x => x.ChatterUserId == user.Id));
        _roomRepository.Update(room);
    }

    public async Task DeleteRoomAsync(DeleteRoomInput deleteRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomPermissions)
            .FirstOrDefaultAsync(x => x.Id == deleteRoomInput.RoomId);

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var user = await _userManager.FindByIdAsync(deleteRoomInput.UserId);
        if (user is null)
            throw new FriendlyException("Kullanıcı bulunamadı");

        if (!(IsFullAdmin(deleteRoomInput.UserId) || room.RoomPermissions
                    .FirstOrDefault(x => x.ChatterUserId == deleteRoomInput.UserId)?.PermissionType ==
                ChatPermissionType.Admin))
            throw new FriendlyException("Chati silme yetkiniz yok");

        _roomRepository.Delete(room);
    }

    public async Task EditRoomAsync(EditRoomInput editRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomPermissions)
            .FirstOrDefaultAsync(x => x.Id == editRoomInput.Id);

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var user = await _userManager.FindByIdAsync(editRoomInput.UserId);
        if (user is null)
            throw new FriendlyException("Kullanıcı bulunamadı");

        if (!(IsFullAdmin(editRoomInput.UserId) || room.RoomPermissions
                    .FirstOrDefault(x => x.ChatterUserId == editRoomInput.UserId)?.PermissionType ==
                ChatPermissionType.Admin))
            throw new FriendlyException("Chat ayarlarını güncelleme yetkiniz yok");

        editRoomInput.Adapt(room);

        _roomRepository.Update(room);
    }

    public async Task AddPermissionToRoomAsync(AddPermissionToRoomInput addPermissionToRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomPermissions)
            .FirstOrDefaultAsync(x => x.Id == addPermissionToRoomInput.RoomId);

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var requesterUser = await _userManager.FindByIdAsync(addPermissionToRoomInput.RequestedUserId);
        if (requesterUser is null)
            throw new FriendlyException("İstek atan kullanıcı bulunamadı");

        if (!(IsFullAdmin(addPermissionToRoomInput.RequestedUserId) || room.RoomPermissions
                    .FirstOrDefault(x => x.ChatterUserId == addPermissionToRoomInput.RequestedUserId)?.PermissionType ==
                ChatPermissionType.Admin))
            throw new FriendlyException("Chate izin verme yetkiniz yok");

        var newChatterUser = await _userManager.FindByIdAsync(addPermissionToRoomInput.ChatterUserId);
        if (newChatterUser is null)
            throw new FriendlyException("İşlem yapılmak istenilen kullanıcı bulunamadı");

        var existPermission =
            room.RoomPermissions.FirstOrDefault(x => x.ChatterUserId == addPermissionToRoomInput.ChatterUserId);

        if (existPermission is null)
            throw new FriendlyException("İşlem yapılmak istenilen kullanıcı chatte değil.");

        existPermission!.PermissionType = addPermissionToRoomInput.PermissionType;

        _roomRepository.Update(room);
    }

    public async Task RemoveUserInRoomAsync(RemoveUserInRoomInput removeUserInRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomPermissions)
            .Include(x => x.RoomChatterUsers)
            .FirstOrDefaultAsync(x => x.Id == removeUserInRoomInput.RoomId);

        if (room is null)
            throw new FriendlyException("Chat bulunamadı");

        var requesterUser = await _userManager.FindByIdAsync(removeUserInRoomInput.RequestedUserId);
        if (requesterUser is null)
            throw new FriendlyException("İstek atan kullanıcı bulunamadı");

        if (!(IsFullAdmin(removeUserInRoomInput.RequestedUserId) || room.RoomPermissions
                    .FirstOrDefault(x => x.ChatterUserId == removeUserInRoomInput.RequestedUserId)?.PermissionType ==
                ChatPermissionType.Admin))
            throw new FriendlyException("Chatten atma yetkiniz yok");

        var deletedInRoomChatterUser = await _userManager.FindByIdAsync(removeUserInRoomInput.ChatterUserId);
        if (deletedInRoomChatterUser is null)
            throw new FriendlyException("İşlem yapılmak istenilen kullanıcı bulunamadı");

        var existPermission =
            room.RoomPermissions.FirstOrDefault(x => x.ChatterUserId == removeUserInRoomInput.ChatterUserId);

        if (existPermission is null)
            throw new FriendlyException("İşlem yapılmak istenilen kullanıcı chatte değil.");

        var roomChatterUser = room.RoomChatterUsers.First(x => x.ChatterUserId == removeUserInRoomInput.ChatterUserId);
        roomChatterUser.IsDeleted = true;
        room.RoomPermissions?.Remove(room.RoomPermissions.First(x =>
            x.ChatterUserId == removeUserInRoomInput.ChatterUserId));
        _roomRepository.Update(room);
    }
}