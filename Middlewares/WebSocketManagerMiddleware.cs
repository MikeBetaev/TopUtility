using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Top.IdentifierGenerator;
using Top.WebSocketModel;

namespace Top
{
    /// <summary>
    /// WebSocket middleware
    /// </summary>
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">next delegate for asp.net core pipeline</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public WebSocketManagerMiddleware(RequestDelegate next, CancellationToken cancellationToken)
        {
            _next = next;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Invoke in asp.net core pipeline
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="webSocketHandler">Websocket handler</param>
        /// <param name="idGenerator">Unique Id generator</param>
        public async Task Invoke(HttpContext context, IWebSocketHandler webSocketHandler, IUniqueIdentifierGenerator idGenerator)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = new IdentifiableWebSocket(idGenerator, await context.WebSockets.AcceptWebSocketAsync());

            await webSocketHandler.HandleAsync(socket, _cancellationToken);
        }
    }
}
