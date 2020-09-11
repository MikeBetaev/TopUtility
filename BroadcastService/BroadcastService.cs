using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Top
{
    /// <summary>
    /// Service broadcast to subscribed clients
    /// </summary>
    public class BroadcastService : IBroadcastService
    {
        /// <summary>
        /// Starts broadcast
        /// </summary>
        /// <param name="webSocketHandler">websocket broadcast handler</param>
        /// <param name="cachedProcessesProvider">Container of processes infromation</param>
        public async Task InvokeAsync(IWebSocketHandler webSocketHandler, ICachedProcessesProvider cachedProcessesProvider, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await webSocketHandler.SendMessageToSubscribedAsync(JsonSerializer.Serialize(cachedProcessesProvider.GetProcesses()), cancellationToken);
                await Task.Delay(1000);
            }
        }
    }
}
