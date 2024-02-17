using Chatter.Application.Services.Rooms;
using Chatter.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Application.Extensions;

public static class ConfigureExtension
{
    public static void ConfigureApplications(this IServiceCollection services)
    {
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IUserService, UserService>();
    }
    
}