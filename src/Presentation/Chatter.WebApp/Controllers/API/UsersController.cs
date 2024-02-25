using Chatter.Application.Dtos.Chats;
using Chatter.Application.Services.Chats;
using Chatter.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.WebApp.Controllers.API;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;
    public UsersController(IUserService userService, IChatService chatService)
    {
        _userService = userService;
        _chatService = chatService;
    }

    // GET
    [HttpGet("search/{username}")]
    public async Task<IActionResult> SearchUserByUsername(string username)
    {
        var result = await _userService.GetUsersShortInfoAsync(username);
        return Ok(result);
    }
    
    [HttpGet("/message/sent")]
    public async Task<IActionResult> SendMessage()
    {
        var chatMessage = new SendChatMessageInput
        {
            RoomId = 7,
            SenderUserId = "43639759-31f3-4bdc-a3eb-9eb9ec6e9455",
            Message = "Hello"
        };
        await _chatService.SendMessageAsync(chatMessage);
        return Ok();
    }
    
    [HttpGet("/message/received/rooms/{roomId}")]
    public async Task<IActionResult> GetChatMessages(int roomId)
    {
        var result = await _chatService.GetChatMessagesAsync(roomId);
        return Ok(result);
    }
}