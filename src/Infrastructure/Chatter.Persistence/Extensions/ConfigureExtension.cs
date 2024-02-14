using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Persistence.Application.Context;
using Chatter.Persistence.RepositoryManagement.Base;
using Chatter.Persistence.RepositoryManagement.EfCore.Base;
using Chatter.Persistence.RepositoryManagement.EfCore.Invitations;
using Chatter.Persistence.RepositoryManagement.EfCore.Rooms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chatter.Persistence.Extensions;

public static class ConfigureExtension
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        // services.AddSingleton(typeof(EfCoreBaseRepository<,>));
        // services.AddSingleton(typeof(NoSqlBaseRepository<>));
        services.AddScoped(typeof(IBaseRepository<,>), typeof(EfCoreBaseRepository<,>));
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();  
    }

    public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<SignInManager<ChatterUser>>();
        services.TryAddScoped<UserManager<ChatterUser>>();

        services.AddIdentityCore<ChatterUser>()
            .AddRoles<IdentityRole>()
            .AddSignInManager<SignInManager<ChatterUser>>()
            .AddUserManager<UserManager<ChatterUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

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

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            options.User.RequireUniqueEmail = true;
            //TODO: Email gönderme işlemi yapılınca burası true olarak değiştirilecek
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = true;
        });
    }

    private static void AddDefaultTokenProviders(this IdentityBuilder builder)
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