using FluentFrame.Service.Shell;
using FluentFrame.UI.Shell;
using FluentFrame.ViewModel.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluentFrame;

public static class HostBuilder
{
    public static IHostBuilder UseFluentFrame(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            //窗口
            services.AddSingleton<IFluentShellService, FluentShellService>();

            //窗口
            services.AddSingleton<FluentShell>();
            services.AddTransient<FluentShellViewModel>();
        });

        return builder;
    }

    public static FluentShell GetFluentShell(this IServiceProvider service) =>
      service.GetRequiredService<FluentShell>();
    public static IFluentShellService GetFluentShellService(this IServiceProvider service) =>
        service.GetRequiredService<IFluentShellService>();
}
