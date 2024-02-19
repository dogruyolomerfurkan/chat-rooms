using Microsoft.AspNetCore.Identity;

namespace Chatter.WebApp.Extensions;

public static class AuthenticationExtension
{
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, options => { options.SlidingExpiration = true; })
            .AddCookie(IdentityConstants.ExternalScheme, options => { options.SlidingExpiration = true; })
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, options => { options.SlidingExpiration = true; });


        services.ConfigureApplicationCookie(options =>
        {
            //ReturnUrlParameter requires 
            options.Cookie.Name = IdentityConstants.ApplicationScheme;
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";
            options.AccessDeniedPath = "/account/accessdenied";

            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(2);
            options.SlidingExpiration = true;
            options.Cookie = new CookieBuilder
            {
                //scriptlerin cookie yi okumasını sağlar
                HttpOnly = true,
                Name = ".Chatter.Security.Cookie",
                SameSite = SameSiteMode.Strict
            };
        });
    }
}