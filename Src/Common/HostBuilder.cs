using Common.Service.File;
using Common.Service.Setting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common;

public static class HostBuilder
{
    /// <summary>
    /// 使用 Common
    /// </summary>
    /// <returns></returns>
    public static IHostBuilder UseCommon(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            //配置
            services.AddSingleton<ISettingProvider, SettingService>();
            services.AddHostedService(x => x.GetRequiredService<ISettingProvider>());
            services.AddSingleton(typeof(ISetter<>), typeof(Setter<>));

            //Fit
            services.AddSingleton<IFitService, FitService>();
            //Gpx
            services.AddSingleton<IGpxService, GpxService>();
        });

        return builder;
    }


    /// <summary>
    /// 获取配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <returns></returns>
    public static ISetter<T> GetSetting<T>(this IServiceProvider service) =>
        service.GetRequiredService<ISetter<T>>();

    /// <summary>
    /// 获取日志记录器
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static ILogger<T> GetLogger<T>(this IServiceProvider service) =>
        service.GetRequiredService<ILogger<T>>();


    /// <summary>
    /// 获取 Fit 服务
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IFitService GetFitService(this IServiceProvider service) =>
        service.GetRequiredService<IFitService>();

    /// <summary>
    /// 获取 Gpx 服务
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IGpxService GetGpxService(this IServiceProvider service) =>
        service.GetRequiredService<IGpxService>();
}