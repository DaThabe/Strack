using Microsoft.Extensions.Hosting;

namespace Strack.Service;

public abstract class OneTimeHostedService : IHostedService
{
    private int _hasStarted = 0; // 0 未启动，1 启动过

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (Interlocked.Exchange(ref _hasStarted, 1) == 1)
        {
            // 已经执行过，不再重复执行
            return;
        }

        await ExecuteOnceAsync(cancellationToken);
    }

    protected abstract Task ExecuteOnceAsync(CancellationToken cancellationToken);
}