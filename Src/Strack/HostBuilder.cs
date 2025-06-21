using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Data;
using Strack.Service;
using Strack.Service.Activity;
using Strack.Service.Migrate;
using Strack.Service.User;

namespace Strack;

public static class HostBuilder
{
    /// <summary>
    /// 使用Strack
    /// </summary>
    /// <returns></returns>
    public static IHostBuilder UseStrack(this IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(x =>
        {
            x.SetBasePath(AppContext.BaseDirectory);
            x.AddJsonFile("appsettings.json", true, true);
        });

        builder.ConfigureServices((context, services) =>
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

            //活动
            services.AddSingleton<IActivityProvider, ActivityProvider>();
            services.AddSingleton<IActivityImportService, ActivityImportService>();
            services.AddSingleton<IActivitySyncService, ActivitySyncService>();

            //用户
            services.AddSingleton<IUserCredentialService, UserCredentialService>();

            //数据库
            services.AddSingleton<IStrackDbService, StrackDbService>();
        });

        return builder;
    }

    public static StrackDbContext GetStrackDbContext(this IServiceProvider service) =>
        service.GetRequiredService<StrackDbContext>();
}