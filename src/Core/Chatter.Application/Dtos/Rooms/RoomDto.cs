using Chatter.Application.Dtos.Invitations;
using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Application.Dtos.Rooms;

public class RoomDto
{
    /// <summary>
    ///    Oda Id'si
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///  Oda oluşma tarihi
    /// </summary>
    public DateTime CreatedDate { get; set; }

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
    /// Oda içindeki user ların yetkileri
    /// </summary>
    public List<RoomPermissionDto>? RoomPermissions { get; set; }

    /// <summary>
    /// Oda davetiyeleri
    /// </summary>
    public List<InvitationDto>? Invitations { get; set; }

    /// <summary>
    /// Odadaki kullanıcılar
    /// </summary>
    public List<RoomChatterUser>? Users { get; set; }

    /// <summary>
    /// Bloklu kullanıcılar
    /// </summary>
    public List<RoomChatterUser>? BlockedUsers { get; set; }
    
}