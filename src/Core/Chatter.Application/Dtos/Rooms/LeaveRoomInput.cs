namespace Chatter.Application.Dtos.Rooms;

public class LeaveRoomInput
{
    /// <summary>
    /// Oda Id'si
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// User Id'si
    /// </summary>
    public string UserId { get; set; }
    
}