using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Model.Database;
using Strack.Service;
using Strack.Service.Importer;
using Strack.Service.Migrate;

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
                x.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                //数据库
                services.AddDbContext<StrackDbContext>((services, builder) =>
                {
                    var connectString = context.Configuration.GetValue<string>("Database:ConnectString");
                    if (string.IsNullOrWhiteSpace(connectString))
                    {
                        //throw new InvalidOperationException("无法连接数据库, 连接参数为空");
                    }

                    builder.UseSqlite(connectString);
                });

                //数据库库迁移
                services.AddHostedService<MigrateHostedService>();

                //行者导入
                services.AddSingleton<IXingzheImport, XingzheImport>();

                //Gpx 同步
                services.AddSingleton<IGpxSyncService, GpxSyncService>();
            });

        return builder;
    }

    public static StrackDbContext GetStrackDbContext(this IServiceProvider service) =>
        service.GetRequiredService<StrackDbContext>();

    public static IXingzheImport GetIXingzheImportService(this IServiceProvider service) =>
        service.GetRequiredService<IXingzheImport>();

    public static IGpxSyncService GetGpxSyncService(this IServiceProvider service) =>
        service.GetRequiredService<IGpxSyncService>();
}