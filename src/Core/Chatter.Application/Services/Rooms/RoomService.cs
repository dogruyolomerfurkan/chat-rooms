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
            .Include(x => x.RoomPermissions)
            .ThenInclude(x => x.ChatterUser)
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.ChatterUser)
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(3)).ToListAsync();
    }

    public async Task<RoomDto> GetRoomByIdAsync(int roomId)
    {
        var room = await _roomRepository.FindAsync(roomId);
        return room.Adapt<RoomDto>();
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomInput createRoomInput)
    {
        var room = createRoomInput.Adapt<Room>();
        var roomPermission = new RoomPermission()
        {
            ChatterUserId = createRoomInput.Users.First().Id,
            RoomId = room.Id,
            PermissionType = PermissionType.Admin,
        };
        room.RoomPermissions?.Add(roomPermission);
        var roomChatterUser = new RoomChatterUser()
        {
            ChatterUserId = createRoomInput.Users.First().Id,
            RoomId = room.Id,
        };
        room.RoomChatterUsers?.Add(roomChatterUser);
        await _roomRepository.CreateAsync(room);

        var dto =  room.Adapt<RoomDto>(CreateTypeAdapterConfig(3));
        return dto;
    }

    public async Task BlockUserByRoomAsync(int roomId, ChatterUser blockedUser)
    {
        var room = await _roomRepository.FindAsync(roomId);
        if (room is null)
            throw new FriendlyException("Room not found");

        // room.BlockedUsers.Add(blockedUser);
    }
}