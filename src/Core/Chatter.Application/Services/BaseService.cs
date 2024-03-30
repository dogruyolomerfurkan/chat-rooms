using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Chatter.Application.Services;

public class BaseService
{
    protected readonly UserManager<ChatterUser> _userManager;

    public BaseService(UserManager<ChatterUser> userManager)
    {
        _userManager = userManager;
    }
    
    protected TypeAdapterConfig CreateTypeAdapterConfig(int maxDepth = 5)
    {
        var config = new TypeAdapterConfig();
        config.Default.PreserveReference(true);
        config.Default.MaxDepth(maxDepth);
        return config;
    }
    
    protected bool IsFullAdmin(string userId)
    {
        var user = _userManager.FindByIdAsync(userId).Result;
        var userRoles = _userManager.GetRolesAsync(user).Result;

        return userRoles.Any(x => x == ChatPermissionType.Admin.ToString());
    }
   
}