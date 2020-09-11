using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Top
{
    /// <summary>
    /// Service provider for collect and update list of processes periodically
    /// </summary>
    public class CachedProcessesProvider : ICachedProcessesProvider
    {
        private readonly ConcurrentDictionary<int, ProcessInformation> _processes = new ConcurrentDictionary<int, ProcessInformation>();

        /// <summary>
        /// Starts updating processes information
        /// </summary>
        public async Task InvokeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var processes = Process.GetProcesses();
                var idHashset = processes.Select(p => p.Id).ToHashSet();
                foreach (var process in _processes.Where(p => !idHashset.Contains(p.Key)))
                {
                    _processes.TryRemove(process.Key, out var processInformation);
                }
                foreach (var process in processes)
                {
                    var processInfo = new ProcessInformation
                    {
                        Id = process.Id,
                        Name = process.ProcessName,
                        Memory = process.PrivateMemorySize64 / 1024
                    };
                    _processes.AddOrUpdate(process.Id, processInfo, (key, oldValue) => processInfo);
                }
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Get-method for processes information
        /// </summary>
        /// <returns>Read-only collection of processes information</returns>
        public IReadOnlyCollection<ProcessInformation> GetProcesses()
        {
            return _processes.Values.OrderBy((x)=>x.Id).ToArray();
        }
    }
}
