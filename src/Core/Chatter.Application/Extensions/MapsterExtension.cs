using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Chatter.Application.Extensions;

public static class MapsterExtension
{
    public static void AddMapsterConfigurations(this IServiceCollection services)
    {
        var configuration = TypeAdapterConfig.GlobalSettings;
        configuration.Default.PreserveReference(true);
        configuration.Default.MaxDepth(3);
        
        configuration.Scan(Assembly.GetExecutingAssembly());
 
        services.AddSingleton(configuration);
 
    }
}