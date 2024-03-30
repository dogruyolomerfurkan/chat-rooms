namespace Chatter.WebApp.Models.Room;

public class InviteUsersToRoomInput
{
    public int RoomId { get; set; }
    public string UserIds { get; set; }
    
}