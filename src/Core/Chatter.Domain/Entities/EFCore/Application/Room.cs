using Chatter.Domain.Entities.EFCore.Application.Base;

namespace Chatter.Domain.Entities.EFCore.Application;

public class Room : BaseEntity<int>
{
    public Room()
    {
        Invitations = new List<Invitation>();
        RoomPermissions = new List<RoomPermission>();
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

    /// <summary>
    ///  Oda içindeki user tipleri
    /// </summary>
    public List<RoomPermission> RoomPermissions { get; set; }
    
    /// <summary>
    /// Oda davetiyeleri
    /// </summary>
    public List<Invitation>? Invitations { get; set; }

    /// <summary>
    /// Odadaki kullanıcılar
    /// </summary>
    public List<RoomChatterUser> RoomChatterUsers { get; set; } = new List<RoomChatterUser>();


}