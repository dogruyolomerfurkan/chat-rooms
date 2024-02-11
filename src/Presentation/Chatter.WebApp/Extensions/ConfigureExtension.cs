using Chatter.Application.Extensions;
using Chatter.Persistence.Extensions;

namespace Chatter.WebApp.Extensions;

public static class ConfigureExtension
{
    public static void ConfigureWebApps(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureIdentity(configuration);
        services.ConfigureDatabase(configuration);
        services.ConfigureAuthentication();
        services.ConfigureApplications();
        
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddMvc();

    }
    
}