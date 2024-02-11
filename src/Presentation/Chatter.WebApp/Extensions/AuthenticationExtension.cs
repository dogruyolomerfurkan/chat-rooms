using Microsoft.AspNetCore.Identity;

namespace Chatter.WebApp.Extensions;

public static class AuthenticationExtension
{
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme= IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        }).AddCookie(IdentityConstants.ApplicationScheme);
        services.ConfigureApplicationCookie(options =>
        {
            //ReturnUrlParameter requires 
            options.Cookie.Name = IdentityConstants.ApplicationScheme;
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";

            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
            options.SlidingExpiration = true;
        });
    }
}