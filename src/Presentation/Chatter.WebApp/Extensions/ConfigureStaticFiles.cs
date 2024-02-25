using Microsoft.Extensions.FileProviders;

namespace Chatter.WebApp.Extensions;

public static class ConfigureStaticFiles
{
    public static IApplicationBuilder CustomStaticFiles(this IApplicationBuilder app)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "node_modules");

        var options = new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(path),
            RequestPath = "/modules"
        };
        app.UseStaticFiles(options);
        return app;
    }
}