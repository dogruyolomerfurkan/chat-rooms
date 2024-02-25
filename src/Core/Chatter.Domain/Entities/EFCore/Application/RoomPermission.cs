using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Enums;

namespace Chatter.Domain.Entities.EFCore.Application;

public class RoomPermission
{
    public int Id { get; set; }

    /// <summary>
    ///  Oda Id'si
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    ///  Odaya ait izin verilen kullanıcının Id'si
    /// </summary>
    public string ChatterUserId { get; set; }

    /// <summary>
    ///  User'ın odaya ait izin tipi
    /// </summary>
    public ChatPermissionType PermissionType { get; set; }

    /// <summary>
    /// Odaya ait iznin veriliş tarihi
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.Now;


    public Room Room { get; set; }
    public ChatterUser ChatterUser { get; set; }
}