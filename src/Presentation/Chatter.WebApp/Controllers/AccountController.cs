using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using Chatter.Common.Exceptions;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Enums;
using Chatter.WebApp.Extensions;
using Chatter.WebApp.Models;
using Chatter.WebApp.Models.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chatter.WebApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ChatterUser> _userManager;
    private readonly SignInManager<ChatterUser> _signInManager;
    // GET
    public AccountController(UserManager<ChatterUser> userManager, SignInManager<ChatterUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Info()
    {
        var user = await _userManager.GetUserAsync(User);
        user.ProfileImagePath ??= "default_img_orange.jpg";
        return View("Edit", user);
    }
    [HttpPost]
    public async Task<IActionResult> EditProfile(ChatterUser chatterUser, IFormFile profileImage)
    {
        if (chatterUser.Id != _userManager.GetUserId(User))
            return BadRequest();
        
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
        user.StatusDescription = chatterUser.StatusDescription;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Güncellendi",
                Message = "Hesap başarılı bir şekilde güncellendi. ",
                Css = "success"
            });
            return RedirectToAction("Index", "Room");
        }
        else
        {
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu lütfen tekrar deneyiniz");
        }

        return RedirectToAction("Index","Room");
    }

    public IActionResult Login(string? ReturnUrl = null)
    {
        var loginDto = new LoginDto();
        loginDto.ReturnUrl = ReturnUrl;
        return View(loginDto);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        ChatterUser user = null;
        try
        {
            MailAddress m = new MailAddress(loginDto.Email);
            user = await _userManager.FindByEmailAsync(loginDto.Email);

        }
        catch (Exception e)
        {
            user = await _userManager.FindByNameAsync(loginDto.Email);
        }
        
        if (user is null)
            throw new FriendlyException("Kullanıcı bulunamadı.");

        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        if (result.Succeeded)
        {
            return Redirect(loginDto.ReturnUrl ?? "~/");
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        TempData.Put("message", new ResultMessage()
        {
            Title = "Oturum Kapatıldı.",
            Message = "Hesabınız güvenli bir şekilde sonlandırıldı",
            Css = "warning"
        });

        return Redirect("~/");
    }

    [HttpGet]
    public IActionResult Register()
    {
        var rgt = new RegisterDto();
        ViewBag.IsRegister = true;
        return View("Login", rgt);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        ViewBag.IsRegister = true;
        if (!ModelState.IsValid)
        {
            return View("Login", model);
        }

        var user = new ChatterUser()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email,
            ProfileImagePath = "default_img_orange.jpg",
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, ChatPermissionType.Chatter.ToString());

            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Bilinmeyen bir hata oluştu lütfen tekrar deneyiniz");
                return View("Login", model);
            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Açılışı",
                Message = "Hesap başarılı bir şekilde oluşturuldu. ",
                Css = "warning"
            });
            ViewBag.IsRegister = false;
            return RedirectToAction("Login", "Account");
        }
        else
        {
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu lütfen tekrar deneyiniz");
        }

        return View("Login", model);
    }
}