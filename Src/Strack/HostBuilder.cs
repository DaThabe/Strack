using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.External.Xingzhe;
using Strack.Model.Database;
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
        builder.ConfigureServices((context, services) =>
        {
            //数据库
            services.AddDbContext<StrackDbContext>((services, builder) =>
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

            //业务
            services.AddSingleton<IXingzheImportService, XingzheImportService>();
            services.AddSingleton<IXingzheApi, XingzheApi>();

            services.AddSingleton<IXingzheHttpClient, XingzheHttpClient>();
            services.Configure<HttpClientStrings>(context.Configuration.GetSection("Api"));
            services.AddHostedService(x => x.GetRequiredService<IXingzheHttpClient>());

        });

        builder.ConfigureHostConfiguration(x =>
        {
            x.SetBasePath(AppContext.BaseDirectory);
            x.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        });

        return builder;
    }


    public static IXingzheApi GetXingzheApi(this IServiceProvider service) => 
        service.GetRequiredService<IXingzheApi>();
    public static IXingzheImportService GetIXingzheImportService(this IServiceProvider service) => 
        service.GetRequiredService<IXingzheImportService>();

    public static StrackDbContext GetStrackDbContext(this IServiceProvider service) =>
        service.GetRequiredService<StrackDbContext>();
}