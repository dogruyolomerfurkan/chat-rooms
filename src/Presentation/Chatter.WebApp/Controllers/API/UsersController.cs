using Chatter.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.WebApp.Controllers.API;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET
    [HttpGet("search/{username}")]
    public async Task<IActionResult> SearchUserByUsername(string username)
    {
        var result = await _userService.GetUsersShortInfoAsync(username);
        return Ok(result);
    }
}