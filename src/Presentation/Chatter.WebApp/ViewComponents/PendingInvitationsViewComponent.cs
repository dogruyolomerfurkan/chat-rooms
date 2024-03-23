using Chatter.Application.Services.Invites;
using Chatter.Application.Services.Users;
using Chatter.Domain.Entities.EFCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chatter.WebApp.ViewComponents;

public class PendingInvitationsViewComponent : ViewComponent
{
    private readonly UserManager<ChatterUser> _userManager;
    private readonly IInvitationService _invitationService;

    public PendingInvitationsViewComponent(IInvitationService invitationService, UserManager<ChatterUser> userManager)
    {
        _invitationService = invitationService;
        _userManager = userManager;
    }

    [HttpGet]
    public IViewComponentResult Invoke()
    {
        var user = _userManager.GetUserAsync(UserClaimsPrincipal).Result;
        var pendingInvitations = _invitationService.GetPendingInvitationsAsync(user.Id).Result;
        return View("_pendingInvitations", pendingInvitations);
    }
}