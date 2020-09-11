using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Top.IdentifierGenerator;

namespace Top.WebSocketModel
{
    public class IdentifiableWebSocket : WebSocket
    {
        private readonly WebSocket _socket;

        public IdentifiableWebSocket(IUniqueIdentifierGenerator idGenerator, WebSocket socket)
        {
            _socket = socket;

            Id = idGenerator.Next();
        }

        public override WebSocketCloseStatus? CloseStatus => _socket.CloseStatus;

        public override string CloseStatusDescription => _socket.CloseStatusDescription;

        public override WebSocketState State => _socket.State;

        public override string SubProtocol => _socket.SubProtocol;

        public override void Abort()
        {
            _socket.Abort();
        }

        public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            return _socket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            return _socket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override void Dispose()
        {
            _socket.Dispose();
        }

        public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return _socket.ReceiveAsync(buffer, cancellationToken);
        }

        public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            return _socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public Task SendMessageAsync(string message, CancellationToken cancellationToken)
        {
            if (_socket.State == WebSocketState.Open)
            {
                var buffer = new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length);

                return _socket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
            }

            return Task.CompletedTask;
        }

        public string Id { get; }

        public bool Subscribed { get; set; }
    }
}
