using IGPSport.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IGPSport;

public static class HostBuilder
{
    /// <summary>
    /// 使用 IGPSport
    /// </summary>
    /// <returns></returns>
    public static IHostBuilder UseIGPSport(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            //客户端容器
            services.AddSingleton<IIGPSportClientProvider, IGPSportClientProvider>();
            //设置
            services.AddSingleton<IIGPSportSetting, IGPSportSetting>();
        });

        return builder;
    }

    /// <summary>
    /// 获取iGPSPORT请求客户端容器
    /// </summary>
    public static IIGPSportClientProvider GetIGPSportClientProvider(this IServiceProvider service) => 
        service.GetRequiredService<IIGPSportClientProvider>();

    /// <summary>
    /// 获取iGPSPORT设置
    /// </summary>
    public static IIGPSportSetting GetIGPSportSetting(this IServiceProvider service) =>
        service.GetRequiredService<IIGPSportSetting>();
}