using Chatter.Application.Dtos.Rooms;
using Chatter.Common.Exceptions;
using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Enums;
using Chatter.Persistence.RepositoryManagement.EfCore.Invitations;
using Chatter.Persistence.RepositoryManagement.EfCore.Rooms;
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

    public RoomService(IRoomRepository roomRepository, IInvitationRepository invitationRepository,
        UserManager<ChatterUser> userManager)
    {
        _roomRepository = roomRepository;
        _invitationRepository = invitationRepository;
        _userManager = userManager;
    }

    public async Task<List<RoomDto>> GetRoomsAsync()
    {
        return await _roomRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(3)).ToListAsync();
    }

    public async Task<List<RoomDto>> GetRoomsByUserIdAsync(string userId)
    {
        return await _roomRepository.Query()
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.Room).SelectMany(x => x.RoomChatterUsers)
            .Where(x => x.ChatterUser.Id == userId)
            .Select(x => x.Room)
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(5)).ToListAsync();
    }

    public async Task<List<RoomDto>> GetPublicRooms()
    {
        return await _roomRepository.Query()
            .Where(x => x.IsPublic)
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
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

        // return room?.Adapt<RoomDto>();
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