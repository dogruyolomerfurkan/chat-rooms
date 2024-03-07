namespace Chatter.WebApp.HUB;

public class ActiveConnections
{

    public static List<SignalRConnection> SignalRConnections { get; set; } = new List<SignalRConnection>();
}

public class SignalRConnection
{
    public string UserId { get; set; }
    public string ConnectionId { get; set; }
}