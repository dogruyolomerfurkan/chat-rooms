using Chatter.Application.Dtos.Rooms;
using Chatter.Common.Exceptions;
using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Persistence.RepositoryManagement.EfCore.Invitations;
using Chatter.Persistence.RepositoryManagement.EfCore.Rooms;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Application.Services.Rooms;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IInvitationRepository _invitationRepository;
    

    public RoomService(IRoomRepository roomRepository, IInvitationRepository invitationRepository)
    {
        _roomRepository = roomRepository;
        _invitationRepository = invitationRepository;
    }

    public async Task<List<RoomDto>> GetRoomsAsync()
    {
        return await _roomRepository.Query().ProjectToType<RoomDto>().ToListAsync();
    }

    public async Task<RoomDto> GetRoomByIdAsync(int roomId)
    {
        var room = await _roomRepository.FindAsync(roomId);
        return room.Adapt<RoomDto>();
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomInput createRoomInput)
    {
        var room = createRoomInput.Adapt<Room>();
        await _roomRepository.CreateAsync(room);
        return room.Adapt<RoomDto>();
    }

    public async Task BlockUserByRoomAsync(int roomId, ChatterUser blockedUser)
    {
        var room = await _roomRepository.FindAsync(roomId);
        if(room is null)
            throw new FriendlyException("Room not found");
        
        room.BlockedUsers.Add(blockedUser);
    }
   
}