using System.Threading;
using System.Threading.Tasks;
using Top.WebSocketModel;

namespace Top
{
    public interface IWebSocketHandler
    {
        Task HandleAsync(IdentifiableWebSocket socket, CancellationToken cancellationToken);

        Task SendMessageToSubscribedAsync(string message, CancellationToken cancellationToken);
    }
}
