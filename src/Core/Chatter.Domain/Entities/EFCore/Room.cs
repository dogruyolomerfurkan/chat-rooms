using Chatter.Domain.Entities.EFCore.Base;
using Chatter.Domain.Enums;
using Chatter.Identity.Entities;

namespace Chatter.Domain.Entities.EFCore;

public class Room : BaseEntity<int>
{
    /// <summary>
    /// Oda başlığı
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Odadaki kullanıcılar
    /// </summary>
    public List<ChatterUser> Users { get; set; }

    /// <summary>
    /// Oda herkese görünür mü
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// Oda kapasitesi
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Odadaki yetkili kullanıcılar ve yetkileri
    /// </summary>
    public Dictionary<string, PermissionType> RoomPermissions { get; set; }

    /// <summary>
    /// Oda davetiyeleri
    /// </summary>
    public List<Invitation> Invitations { get; set; }
    
    /// <summary>
    /// Bloklu kullanıcılar
    /// </summary>
    public List<ChatterUser> BlockedUsers { get; set; }
    
}