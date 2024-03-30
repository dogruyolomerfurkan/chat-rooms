using Chatter.Application.Services.Rooms;
using Chatter.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.WebApp.Controllers.API;

[ApiController]
[Route("api/v1")]
public class APIController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRoomService _roomService;
    public APIController(IUserService userService, IRoomService roomService)
    {
        _userService = userService;
        _roomService = roomService;
    }

    // GET
    [HttpGet("users/search/{username}")]
    public async Task<IActionResult> SearchUserByUsername(string username)
    {
        var result = await _userService.GetUsersShortInfoAsync(username);
        return Ok(result);
    }
    
}