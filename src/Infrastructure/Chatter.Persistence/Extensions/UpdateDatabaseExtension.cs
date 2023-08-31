using Chatter.Persistence.Application.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Persistence.Extensions;

public static class UpdateDatabaseExtension
{
    public static void UpdateApplicationDb(this IApplicationBuilder app, bool useMigrate = true)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            try
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    
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