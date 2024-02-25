using Chatter.Application.Dtos.Rooms;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Application.Services.Rooms;

public interface IRoomService
{
    /// <summary>
    /// admin veya başka işlemler için bütün odalar
    /// </summary>
    /// <returns></returns>
    Task<List<RoomDto>> GetRoomsAsync();

    /// <summary>
    /// Id'ye göre oda getir
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task<RoomDto?> GetRoomDetailAsync(int roomId);
    
    /// <summary>
    /// Kullanıcının olduğu odalar
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<RoomDto>> GetRoomsByUserIdAsync(string userId);
    
    /// <summary>
    /// Anasafaya vb. için public odalar
    /// </summary>
    /// <returns></returns>
    
    Task<List<RoomDto>> GetPublicRooms();
    
    /// <summary>
    /// Yeni oda oluştur
    /// </summary>
    /// <param name="createRoomInput"></param>
    /// <returns></returns>
    Task<RoomDto> CreateRoomAsync(CreateRoomInput createRoomInput);

    /// <summary>
    /// Bir kişinin odayı görmesini engeller
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="blockedUser"></param>
    /// <returns></returns>
    //TODO: sonraki aşamada inşa edilecek bir yapı için
    Task BlockUserByRoomAsync(int roomId, ChatterUser blockedUser);
    
    /// <summary>
    /// Odaya katılır
    /// </summary>
    /// <param name="joinRoomInput"></param>
    /// <returns></returns>
    Task JoinRoomAsync(JoinRoomInput joinRoomInput);
    
    /// <summary>
    /// Odayı terk eder
    /// </summary>
    /// <param name="leaveRoomInput"></param>
    /// <returns></returns>
    Task LeaveRoomAsync(LeaveRoomInput leaveRoomInput);
    
    /// <summary>
    /// Odayı siler
    /// </summary>
    /// <param name="deleteRoomInput"></param>
    /// <returns></returns>
    Task DeleteRoomAsync(DeleteRoomInput deleteRoomInput);
    
    Task EditRoomAsync(EditRoomInput editRoomInput);
}