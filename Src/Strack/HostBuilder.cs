using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Data;
using Strack.Service.Import;
using Strack.Service.Migrate;
using Strack.Service.Sync;

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
                //services.AddSingleton<ISyncService, SyncService>();

                //储存库
                services.AddSingleton<IActivityImportService, ActivityImportService>();
                services.AddSingleton<ISyncFactoryService, SyncFactoryService>();
            });

        return builder;
    }

    public static StrackDbContext GetStrackDbContext(this IServiceProvider service) =>
        service.GetRequiredService<StrackDbContext>();
    public static IActivityImportService GetActivityImportService(this IServiceProvider service) =>
        service.GetRequiredService<IActivityImportService>();

    public static ISyncFactoryService GetSyncFactoryService(this IServiceProvider service) =>
        service.GetRequiredService<ISyncFactoryService>();
}