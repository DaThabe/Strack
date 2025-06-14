using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Model.Database;
using Strack.Service;
using Strack.Service.Migrate;
using Strack.Service.Repository;

namespace Strack;

public static class HostBuilder
{
    /// <summary>
    /// 使用Strack
    /// </summary>
    /// <returns></returns>
    public static IHostBuilder UseStrack(this IHostBuilder builder)
    {
        builder
            .ConfigureHostConfiguration(x=>
            {
                x.SetBasePath(AppContext.BaseDirectory);
                x.AddJsonFile("appsettings.json", true, true);
            })
            .ConfigureServices((context, services) =>
            {
                //数据库
                services.AddDbContextFactory<StrackDbContext>((services, builder) =>
                {
                    var connectString = context.Configuration.GetValue<string>("Database:ConnectString");
                    if (string.IsNullOrWhiteSpace(connectString))
                    {
                        throw new InvalidOperationException("无法连接数据库, 连接参数为空");
                    }

                    builder.UseSqlite(connectString);
                });
                //数据库库迁移
                services.AddHostedService<MigrateHostedService>();


                //同步
                services.AddSingleton<ISyncService, SyncService>();

                //储存库
                services.AddSingleton<IXingZheRepository, XingZheRepository>();
                services.AddSingleton<IIGPSportRepository, IGPSportRepository>();
            });

        return builder;
    }

    public static StrackDbContext GetStrackDbContext(this IServiceProvider service) =>
        service.GetRequiredService<StrackDbContext>();
    public static IXingZheRepository GetXingZheRepository(this IServiceProvider service) =>
        service.GetRequiredService<IXingZheRepository>();
    public static IIGPSportRepository GetIGPSportRepository(this IServiceProvider service) =>
        service.GetRequiredService<IIGPSportRepository>();
}