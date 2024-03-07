using Chatter.Domain.Enums;

namespace Chatter.Application.Dtos.Rooms;

public class AddPermissionToRoomInput
{
    /// <summary>
    ///  Oda Id'si
    /// </summary>
    public int RoomId { get; set; }
    
    /// <summary>
    /// ChatterUserId'ye ait izin veren kullanıcının Id'si
    /// </summary>
    public string RequestedUserId { get; set; }

    /// <summary>
    ///  Odaya ait izin verilen kullanıcının Id'si
    /// </summary>
    public string ChatterUserId { get; set; }

    /// <summary>
    ///  User'ın odaya ait izin tipi
    /// </summary>
    public ChatPermissionType PermissionType { get; set; }
}