using Chatter.Domain.Enums;

namespace Chatter.Application.Dtos.Rooms;

public class RemoveUserInRoomInput
{
    /// <summary>
    ///  Oda Id'si
    /// </summary>
    public int RoomId { get; set; }
    
    /// <summary>
    /// ChatterUserId'yi odadan atan kullanıcının Id'si
    /// </summary>
    public string RequestedUserId { get; set; }

    /// <summary>
    ///  Odaya ait izin verilen kullanıcının Id'si
    /// </summary>
    public string ChatterUserId { get; set; }
}