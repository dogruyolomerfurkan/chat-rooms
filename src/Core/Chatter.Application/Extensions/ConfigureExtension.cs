using Chatter.Application.Services.Rooms;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Application.Extensions;

public static class ConfigureExtension
{
    public static void ConfigureApplications(this IServiceCollection services)
    {
        services.AddScoped<IRoomService, RoomService>();
    }
    
}