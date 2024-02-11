using Chatter.Application.Dtos.Rooms;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Application.Services.Rooms;

public interface IRoomService
{
    Task<List<RoomDto>> GetRoomsAsync();
    
    Task<RoomDto> GetRoomByIdAsync(int roomId);
    
    Task<RoomDto> CreateRoomAsync(CreateRoomInput createRoomInput);

    Task BlockUserByRoomAsync(int roomId, ChatterUser blockedUser);

}