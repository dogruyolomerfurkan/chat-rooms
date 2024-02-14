using Mapster;

namespace Chatter.Application.Services;

public class BaseService
{
    protected TypeAdapterConfig CreateTypeAdapterConfig(int maxDepth = 5)
    {
        var config = new TypeAdapterConfig();
        config.Default.PreserveReference(true);
        config.Default.MaxDepth(maxDepth);
        return config;
    }
   
}