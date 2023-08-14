using Chatter.Identity.Context;
using Chatter.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Identity.Extensions;

public static class ConfigureExtension
{
    
    public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<ChatterUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityDbConnection")));
        //örnek
            // .AddPasswordValidator<CustomPasswordValidation>()
            // .AddUserValidator<CustomUserValidation>()
            // .AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>()
            // .AddDefaultTokenProviders(); 
        
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(0.5);
            
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            options.User.RequireUniqueEmail = true;
            //TODO: Email gönderme işlemi yapılınca burası true olarak değiştirilecek
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });

    }
    
    public static void AddDefaultTokenProviders(this IdentityBuilder builder) 
    { 
        var userType = builder.UserType; 
        var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<>).MakeGenericType(userType); 
        var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(userType); 
        var authenticatorProviderType = typeof(AuthenticatorTokenProvider<>).MakeGenericType(userType); 
        
        builder 
            .AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProviderType) 
            .AddTokenProvider(TokenOptions.DefaultPhoneProvider, phoneNumberProviderType) 
            .AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, authenticatorProviderType); 
    } 
}