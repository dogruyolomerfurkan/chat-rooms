using Chatter.Identity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Identity.Extensions;

public static class SeedExtension
{
    public static void SeedIdentity(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var userManager =
                (UserManager<ChatterUser>) scope.ServiceProvider.GetService(typeof(UserManager<ChatterUser>));
            var roleManager =
                (RoleManager<IdentityRole>) scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));

            string username = "emronr";
            string email = "emre998@hotmail.com";
            string password = "Admin123!+";
            string firstName = "Emre";
            string lastName = "Onur";
            string role = "Admin";

            if (userManager.FindByEmailAsync(username).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(role)).Wait();
                Console.WriteLine("role eklendi");
                var user = new ChatterUser()
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, role).Wait();
                }
            }
        }
    }
}