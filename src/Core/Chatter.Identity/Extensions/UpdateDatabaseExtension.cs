using Chatter.Identity.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Identity.Extensions;

public static class UpdateDatabaseExtension
{
    public static void UpdateIdentityDb(this IApplicationBuilder app, bool useMigrate = true)
    {
            
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            try
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                    
                if(useMigrate)
                    context.Database.Migrate();
                    
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}