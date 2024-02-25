using Chatter.Application.Extensions;
using Chatter.Common.Settings;
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
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddEndpointsApiExplorer(); 
        services.AddMvc(options =>
        {
            options.Filters.Add<CustomErrorAttribute>();
            options.EnableEndpointRouting = true;
        });

        services.AddSignalR();
        
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

    }
    
}