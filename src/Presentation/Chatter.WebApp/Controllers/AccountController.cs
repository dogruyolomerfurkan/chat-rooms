using Chatter.Domain.Entities.EFCore.Identity;
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
}