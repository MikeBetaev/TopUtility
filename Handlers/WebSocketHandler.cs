using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Top.WebSocketModel;

namespace Top
{
    /// <summary>
    /// Class for handling websocket connections
    /// </summary>
    public class WebSocketHandler : IWebSocketHandler
    {
        private readonly ICachedProcessesProvider _provider;
        private readonly IConnectionManager _webSocketConnectionManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webSocketConnectionManager">Storage for sockets</param>
        /// <param name="provider"> Provider to access processes information</param>
        public WebSocketHandler(IConnectionManager webSocketConnectionManager, ICachedProcessesProvider provider)
        {
            _webSocketConnectionManager = webSocketConnectionManager;
            _provider = provider;
        }

        /// <summary>
        /// Handle websocket connection
        /// </summary>
        /// <param name="socket">socket</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task HandleAsync(IdentifiableWebSocket socket, CancellationToken cancellationToken)
        {
            _webSocketConnectionManager.AddSocket(socket);

            await Receive(socket,
                async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        await OnMessageReceive(socket, result, buffer, cancellationToken);
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await OnDisconnected(socket, cancellationToken);
                        return;
                    }
                },
                async (socket) =>
                {
                    await OnDisconnected(socket, cancellationToken);
                },
                cancellationToken
           );
        }

        /// <summary>
        /// Broadcast message for all subscribed clients
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public async Task SendMessageToSubscribedAsync(string message, CancellationToken cancellationToken)
        {
            foreach (var socket in _webSocketConnectionManager.GetSubscribed())
            {
                await socket.SendMessageAsync(message, cancellationToken);
            }
        }

        private async Task Receive(IdentifiableWebSocket socket,
                Action<WebSocketReceiveResult, byte[]> handleMessageCallback, Action<IdentifiableWebSocket> exceptionCallback, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    handleMessageCallback?.Invoke(result, buffer);
                }
                catch (Exception)
                {
                    exceptionCallback(socket);
                }
            }
        }

        private async Task OnDisconnected(IdentifiableWebSocket socket, CancellationToken cancellationToken)
        {
            await _webSocketConnectionManager.RemoveSocket(socket.Id, cancellationToken);
        }

        private async Task OnMessageReceive(IdentifiableWebSocket socket, WebSocketReceiveResult result, byte[] buffer, CancellationToken cancellationToken)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var request = JsonSerializer.Deserialize<ClientRequest>(message);

            if (request.Action == RequestedClientActionEnum.Show)
            {
                await socket.SendMessageAsync(JsonSerializer.Serialize(_provider.GetProcesses()), cancellationToken);
            }
            else if (request.Action == RequestedClientActionEnum.Subscribe)
            {
                socket.Subscribed = true;
            }
            else if (request.Action == RequestedClientActionEnum.Unsubscribe)
            {
                socket.Subscribed = false;
            }
        }
    }
}
