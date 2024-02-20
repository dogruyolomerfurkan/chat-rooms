using Chatter.Application.Dtos.Rooms;
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
using MongoDB.Driver.Linq;

namespace Chatter.Application.Services.Rooms;

public class RoomService : BaseService, IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly UserManager<ChatterUser> _userManager;
    private readonly IUserRepository _userRepository;
    public RoomService(IRoomRepository roomRepository, IInvitationRepository invitationRepository,
        UserManager<ChatterUser> userManager, IUserRepository userRepository)
    {
        _roomRepository = roomRepository;
        _invitationRepository = invitationRepository;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<List<RoomDto>> GetRoomsAsync()
    {
        return await _roomRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(3)).ToListAsync();
    }

    public async Task<List<RoomDto>> GetRoomsByUserIdAsync(string userId)
    {
        var roomIds = await _userRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .SelectMany(x => x.RoomChatterUsers!).Select(x => x.RoomId).ToListAsync();
        
        return await _roomRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .Where(x => roomIds.Contains(x.Id))
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(3)).ToListAsync();
    }

    public async Task<List<RoomDto>> GetPublicRooms()
    {
        return await _roomRepository.Query()
            .Where(x => x.IsPublic)
            .Include(x => x.RoomChatterUsers)
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(3)).ToListAsync();
    }

    public async Task<RoomDto?> GetRoomByIdAsync(int roomId)
    {
        return await _roomRepository.Query()
            .Where(x => x.Id == roomId)
            .Include(x => x.RoomPermissions)
            .ThenInclude(x => x.ChatterUser)
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(3)).FirstOrDefaultAsync();
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

    public async Task BlockUserByRoomAsync(int roomId, ChatterUser blockedUser)
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
            throw new FriendlyException("Oda bulunamadı");
        
        if (room.Capacity == room.RoomChatterUsers.Count)
            throw new FriendlyException("Oda kapasitesi dolu");

        var user = await _userManager.FindByIdAsync(joinRoomInput.UserId);
        if (user is null)
            throw new FriendlyException("Kullanıcı bulunamadı");
        
        if(room.RoomChatterUsers.Select(x => x.ChatterUserId).Contains(user.Id))
            throw new FriendlyException("Kullanıcı zaten odada");

        var roomChatterUser = new RoomChatterUser()
        {
            ChatterUserId = user.Id,
            RoomId = room.Id,
        };
        room.RoomChatterUsers?.Add(roomChatterUser);
        _roomRepository.Update(room);
    }

    public async Task LeaveRoomAsync(LeaveRoomInput leaveRoomInput)
    {
        var room = await _roomRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
            .FirstOrDefaultAsync(x => x.Id == leaveRoomInput.RoomId);
        
        if (room is null)
            throw new FriendlyException("Oda bulunamadı");
        
        var user = await _userManager.FindByIdAsync(leaveRoomInput.UserId);
        if (user is null)
            throw new FriendlyException("Kullanıcı bulunamadı");
        
        if(!room.RoomChatterUsers.Select(x => x.ChatterUserId).Contains(user.Id))
            throw new FriendlyException("Kullanıcı zaten odada değil");
        
        var roomChatterUser = room.RoomChatterUsers.First(x => x.ChatterUserId == user.Id);
        room.RoomChatterUsers?.Remove(roomChatterUser);
        _roomRepository.Update(room);
    }
}