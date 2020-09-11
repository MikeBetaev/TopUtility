using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace Top
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                        PathString path, CancellationToken cancellationToken)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(cancellationToken));
        }
    }
}
