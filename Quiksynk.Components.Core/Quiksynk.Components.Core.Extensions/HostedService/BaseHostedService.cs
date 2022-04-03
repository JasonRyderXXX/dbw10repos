using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quiksynk.Components.Core.HostedService
{
    public abstract class BaseHostedService : IHostedService
    {
        private IHostApplicationLifetime Lifetime { get; }
        protected IConfiguration Config { get; }
        protected ILogger Logger { get; }

        protected abstract void StartApp();
        protected abstract void StopApp();
        protected abstract void StoppingApp();

        public BaseHostedService(IHostApplicationLifetime lifetime, IConfiguration config,ILogger logger )
        {
            Lifetime = lifetime;
            Config = config;
            Logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.Log(LogLevel.Debug, "Application Starting");
            Lifetime.ApplicationStarted.Register(StartApp);
            Lifetime.ApplicationStopped.Register(StopApp);
            Lifetime.ApplicationStopping.Register(StoppingApp);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.Log(LogLevel.Debug, "Application Stopped");
            return Task.CompletedTask;
        }
    }
}
