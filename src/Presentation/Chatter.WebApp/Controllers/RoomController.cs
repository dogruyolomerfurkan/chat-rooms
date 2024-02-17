using Chatter.Application.Dtos.Rooms;
using Chatter.Application.Services.Rooms;
using Chatter.Domain.Entities.EFCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.WebApp.Controllers;

public class RoomController : Controller
{
    private readonly IRoomService _roomService;
    private readonly UserManager<ChatterUser> _userManager;
    // GET
    public RoomController(IRoomService roomService, UserManager<ChatterUser> userManager)
    {
        _roomService = roomService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _roomService.GetRoomsAsync();
        return View(rooms);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateRoomInput createRoomInput)
    {
        // var user =  _userManager.Users.AsNoTracking().First(x => x.UserName == User.Identity.Name);
        var user = await _userManager.GetUserAsync(User);
        createRoomInput.Users.Add(user);
        
        await _roomService.CreateRoomAsync(createRoomInput);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        return View(room);
    }
    
    
}