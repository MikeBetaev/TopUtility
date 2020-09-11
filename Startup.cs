using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Top
{
    public class Startup
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketHandler()
                    .AddWSConnectionManager()
                    .AddCachedProcessesProvider()
                    .AddBroadcastService()
                    .AddUniqueIdentifierGenerator();
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime hostApplicationLifetime, IWebSocketHandler websocketHandler, ICachedProcessesProvider processesProvider, IBroadcastService broadcast)
        {
            hostApplicationLifetime.ApplicationStopping.Register(OnShutdown);

            Task.Run(() => processesProvider.InvokeAsync(tokenSource.Token));
            Task.Run(() => broadcast.InvokeAsync(websocketHandler, processesProvider, tokenSource.Token));

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", tokenSource.Token);
        }

        private void OnShutdown()
        {
            tokenSource.Cancel();
        }
    }
}
