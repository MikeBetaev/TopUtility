using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Top.WebSocketModel;

namespace Top
{
    public interface IConnectionManager
    {
        IEnumerable<IdentifiableWebSocket> GetAll();

        IEnumerable<IdentifiableWebSocket> GetSubscribed();

        IdentifiableWebSocket GetById(string id);

        void AddSocket(IdentifiableWebSocket socket);

        Task RemoveSocket(string id, CancellationToken cancellationToken);

        bool Any();
    }
}
