using Microsoft.Extensions.DependencyInjection;
using Top.IdentifierGenerator;

namespace Top
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddWebSocketHandler(this IServiceCollection services)
        {
            return services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
        }

        public static IServiceCollection AddWSConnectionManager(this IServiceCollection services)
        {
            return services.AddTransient<IConnectionManager, ConnectionManager>();
        }

        public static IServiceCollection AddCachedProcessesProvider(this IServiceCollection services)
        {
            return services.AddSingleton<ICachedProcessesProvider, CachedProcessesProvider>();
        }

        public static IServiceCollection AddBroadcastService(this IServiceCollection services)
        {
            return services.AddSingleton<IBroadcastService, BroadcastService>();
        }
        public static IServiceCollection AddUniqueIdentifierGenerator(this IServiceCollection services)
        {
            return services.AddSingleton<IUniqueIdentifierGenerator, UniqueIdentifierGenerator>();
        }
    }
}
