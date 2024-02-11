using Chatter.Application.Dtos.Invitations;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Application.Dtos.Rooms;

public class CreateRoomInput
{
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
    /// Oda davetiyeleri
    /// </summary>
    public List<CreateInvitationInput>? Invitations { get; set; }

    /// <summary>
    /// Odadaki kullanıcılar
    /// </summary>
    public List<ChatterUser>? Users { get; set; }

    /// <summary>
    /// Bloklu kullanıcılar
    /// </summary>
    public List<ChatterUser>? BlockedUsers { get; set; }
}