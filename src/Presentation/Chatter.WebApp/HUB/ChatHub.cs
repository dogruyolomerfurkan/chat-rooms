using Chatter.Application.Dtos.Chats;
using Chatter.Application.Services.Chats;
using Chatter.Application.Services.Rooms;
using Chatter.Application.Services.Users;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Entities.NoSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Chatter.WebApp.HUB;

public class ChatHub : Hub
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IChatService _chatService;
    private readonly IRoomService _roomService;
    private readonly UserManager<ChatterUser> _userManager;
    private readonly IUserService _userService;

    private List<SignalRConnection> signalRConnections = new List<SignalRConnection>();

    public ChatHub(IHttpContextAccessor httpContextAccessor, IChatService chatService, IRoomService roomService,
        UserManager<ChatterUser> userManager, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _chatService = chatService;
        _roomService = roomService;
        _userManager = userManager;
        _userService = userService;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();

        var userRooms = await _roomService.GetRoomsByUserIdAsync(userId);

        foreach (var userRoom in userRooms)
        {
            await JoinChatRoom(userRoom.Id);
        }

        var signalRConnection = signalRConnections.FirstOrDefault(s => s.UserId == userId);
        if (signalRConnection is null)
        {
            signalRConnections.Add(new()
            {
                UserId = userId!,
                ConnectionId = Context.ConnectionId
            });
        }
        else
        {
            signalRConnection.ConnectionId = Context.ConnectionId;
        }

        await base.OnConnectedAsync();
    }

    public async Task<List<Application.Dtos.Chats.ChatMessage>> GetMessages(int roomId)
    {
        var messages = await _chatService.GetChatMessagesAsync(roomId);
        return messages;
    }

    public async Task SendMessage(string message, int roomId)
    {
        var checkRoom = await _roomService.GetRoomByIdAsync(roomId);
        if (checkRoom is null)
            return;
        var checkUserInRoom = checkRoom?.Users?.FirstOrDefault(x => x.Id == GetUserId());
        if (checkUserInRoom is null)
            return;

        var sendChatMessageInput = new SendChatMessageInput()
        {
            Message = message,
            RoomId = roomId,
            SenderUserId = GetUserId(),
        };
        var chatMessage = await _chatService.SendMessageAsync(sendChatMessageInput);

        var userShortInfo = _userService.GetUsersShortInfoAsync(GetUserName()).Result.FirstOrDefault();

        await Clients.Groups(roomId.ToString()).SendAsync("ChatRoom", chatMessage, userShortInfo, Context.ConnectionId);
    }

    public Task JoinChatRoom(int roomId)
    {
        var singalRConnection = new SignalRConnection
        {
            UserId = GetUserId(),
            ConnectionId = Context.ConnectionId
        };
        signalRConnections.Add(singalRConnection);
        //Group name will be group Id
        //Önce grubu ön tarafta oluştur. Kullanıcıyı gruba ata,
        return Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public async Task LeaveChatRoom(int roomId)
    {
        signalRConnections.RemoveAll(x => x.ConnectionId == Context.ConnectionId && x.UserId == GetUserId());

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        await Clients.Group(roomId.ToString()).SendAsync("ChatRoom", $"{Context.ConnectionId} left {roomId} group.");
    }

    private string GetUserId()
    {
        return _userManager.GetUserId(_httpContextAccessor?.HttpContext?.User);
    }

    private string GetUserName()
    {
        return _userManager.GetUserName(_httpContextAccessor?.HttpContext?.User);
    }
}

public class SignalRConnection
{
    public string UserId { get; set; }
    public string ConnectionId { get; set; }
}