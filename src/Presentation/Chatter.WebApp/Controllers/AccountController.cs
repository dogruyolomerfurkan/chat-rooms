using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.WebApp.Extensions;
using Chatter.WebApp.Models;
using Chatter.WebApp.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    
    public IActionResult Info()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if(user is null)
            throw new Exception("User not found");
        
        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
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
        return View(rgt);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ChatterUser()
        {
            UserName = model.UserName,
            Email = model.Email,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Açışışı",
                Message = "Hesap başarılı bir şekilde oluşturuldu. ",
                Css = "warning"
            });
            return RedirectToAction("Login", "Account");
        }
        else
        {
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu lütfen tekrar deneyiniz");
        }

        return View(model);
    }

}