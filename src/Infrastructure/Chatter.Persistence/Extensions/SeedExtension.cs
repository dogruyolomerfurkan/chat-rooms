using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Persistence.Extensions;

public static class SeedExtension
{
    public static void SeedIdentity(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var userManager =
                (UserManager<ChatterUser>) scope.ServiceProvider.GetService(typeof(UserManager<ChatterUser>))!;
            var roleManager =
                (RoleManager<IdentityRole>) scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))!;

            string username = "emronr";
            string email = "emre998@hotmail.com";
            string password = "Admin123!+";
            string firstName = "Emre";
            string lastName = "Onur";
            
            if (userManager.FindByEmailAsync(username).Result == null)
            {
                var user = new ChatterUser()
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true,
                    ProfileImagePath = username + ".jpg"
                    
                };

                var result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, SeedRoles(roleManager).First(x => x == "Admin")).Wait();
                }
            }
        }
    }

    private static List<String> SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        List<string> roles = new List<string>()
        {
            ChatPermissionType.Admin.ToString(),
            "User",
            ChatPermissionType.Chatter.ToString()
        };

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                IdentityResult roleResult = roleManager.CreateAsync(new IdentityRole(role)).Result;
            }
        }
        return roles;
    }
}