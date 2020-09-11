using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Top
{
    public interface ICachedProcessesProvider
    {
        Task InvokeAsync(CancellationToken cancellationToken);
        IReadOnlyCollection<ProcessInformation> GetProcesses();
    }
}
