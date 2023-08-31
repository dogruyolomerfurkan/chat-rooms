using Chatter.Domain.Entities.EFCore.Application.Base;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Domain.Entities.EFCore.Application;

public class Room : BaseEntity<int>
{
    public Room()
    {
        Invitations = new List<Invitation>();
        Users = new List<ChatterUser>();
        BlockedUsers = new List<ChatterUser>();
    }
    /// <summary>
    /// Oda başlığı
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Oda herkese görünür mü
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// Oda kapasitesi
    /// </summary>
    public int Capacity { get; set; }

    //TODO burası yapılandırılacak
    // /// <summary>
    // /// Odadaki yetkili kullanıcılar ve yetkileri
    // /// </summary>
    // public Dictionary<string, PermissionType> RoomPermissions { get; set; }

    /// <summary>
    /// Oda davetiyeleri
    /// </summary>
    public List<Invitation>? Invitations { get; set; }
    
    /// <summary>
    /// Odadaki kullanıcılar
    /// </summary>
    public List<ChatterUser>? Users { get; set; }
    
    /// <summary>
    /// Bloklu kullanıcılar
    /// </summary>
    public List<ChatterUser>? BlockedUsers { get; set; }
    
}