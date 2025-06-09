using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XingZhe.Service;

namespace XingZhe;

public static class HostBuilder
{
    /// <summary>
    /// 使用 XingZhe
    /// </summary>
    /// <returns></returns>
    public static IHostBuilder UseXingZhe(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<IXingZheClientProvider, XingZheClientProvider>();
            services.AddHostedService(x => x.GetRequiredService<IXingZheClientProvider>());
        });

        return builder;
    }

    /// <summary>
    /// 获取行者请求客户端容器
    /// </summary>
    public static IXingZheClientProvider GetXingZheClientProvider(this IServiceProvider service) =>
        service.GetRequiredService<IXingZheClientProvider>();
}