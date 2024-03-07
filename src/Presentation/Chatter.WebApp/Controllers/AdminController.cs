using Chatter.Application.Services.Rooms;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.WebApp.Extensions;
using Chatter.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chatter.WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IRoomService _roomService;
    private readonly UserManager<ChatterUser> _userManager;

    public AdminController(IRoomService roomService, UserManager<ChatterUser> userManager)
    {
        _roomService = roomService;
        _userManager = userManager;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ListRoom()
    {
        var rooms = await _roomService.GetRoomsAsync();
        return View(rooms);
    }

    public async Task<IActionResult> ListUser()
    {
        var users = await _userManager.Users.ToListAsync();
        return View("User/List", users);
    }

    [HttpGet("/Admin/EditUser/{userId}")]
    public async Task<IActionResult> EditUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        user.ProfileImagePath ??= "default_img_orange.jpg";
        return View("User/Edit", user);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(ChatterUser chatterUser, IFormFile profileImage)
    {
        var user = await _userManager.FindByIdAsync(chatterUser.Id);

        if (profileImage != null)
        {
            user.ProfileImagePath = chatterUser.UserName + ".jpg";

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "profileImages",
                chatterUser.UserName + ".jpg");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }
        }

        user.FirstName = chatterUser.FirstName;
        user.LastName = chatterUser.LastName;
        user.UserName = chatterUser.UserName;
        user.Email = chatterUser.Email;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Güncellendi",
                Message = "Hesap başarılı bir şekilde güncellendi. ",
                Css = "success"
            });
            return RedirectToAction("ListUser");
        }
        else
        {
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu lütfen tekrar deneyiniz");
        }

        return RedirectToAction("ListUser");
    }
}