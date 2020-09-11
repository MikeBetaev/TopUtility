using System.Threading;
using System.Threading.Tasks;

namespace Top
{
    public interface IBroadcastService
    {
        Task InvokeAsync(IWebSocketHandler webSocketHandler, ICachedProcessesProvider cachedProcessesProvider, CancellationToken cancellationToken);
    }
}
