using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Tradeas.Web.Api.Services
{
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Task _task;
        private readonly CancellationTokenSource _cancellationTokenSource;
        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);
        
        public BackgroundService()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _task = ExecuteAsync(_cancellationTokenSource.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_task.IsCompleted)
            {
                return _task;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_task == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _cancellationTokenSource.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_task, Task.Delay(Timeout.Infinite,
                    cancellationToken));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}