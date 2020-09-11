using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Top.WebSocketModel;

namespace Top
{
    /// <summary>
    /// Container for sockets
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        private readonly ConcurrentDictionary<string, IdentifiableWebSocket> _sockets = new ConcurrentDictionary<string, IdentifiableWebSocket>();

        /// <summary>
        /// Return all sockets
        /// </summary>
        /// <returns>List of sockets</returns>
        public IEnumerable<IdentifiableWebSocket> GetAll()
        {
            return _sockets.Values;
        }

        /// <summary>
        /// Return all subscribed sockets
        /// </summary>
        /// <returns>List of sockets</returns>
        public IEnumerable<IdentifiableWebSocket> GetSubscribed()
        {
            return _sockets.Values.Where(s => s.Subscribed);
        }

        /// <summary>
        /// Return socket by id
        /// </summary>
        /// <param name="id">socket id</param>
        /// <returns>socket</returns>
        public IdentifiableWebSocket GetById(string id)
        {
            return _sockets[id];
        }

        /// <summary>
        /// Add socket to list
        /// </summary>
        /// <param name="socket">socket to add</param>
        public void AddSocket(IdentifiableWebSocket socket)
        {
            _sockets.TryAdd(socket.Id, socket);
        }

        /// <summary>
        /// Remove and close socket
        /// </summary>
        /// <param name="id">socket id</param>
        /// <param name="cancellationToken">cancellation token which stop execution if needed</param>
        public async Task RemoveSocket(string id, CancellationToken cancellationToken)
        {
            _sockets.TryRemove(id, out var socket);
            if (socket != null && socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the ConnectionManager", cancellationToken);
                socket.Dispose();
            }
        }

        /// <summary>
        /// Is any sockets already exists
        /// </summary>
        /// <returns>True or false</returns>
        public bool Any()
        {
            return _sockets.Any();
        }
    }
}
