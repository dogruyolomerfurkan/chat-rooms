using Chatter.Application.Dtos.Invitations;
using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Application.Dtos.Rooms;

public class EditRoomInput
{
    /// <summary>
    /// Odanın Id'si
    /// </summary>
    public int Id { get; set; }
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
    /// User Id'si
    /// </summary>
    public string UserId { get; set; }
}