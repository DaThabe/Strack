using Common.Model.Hosted;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Data;

namespace Strack.Service.Migrate;

internal class MigrateHostedService(
    ILogger<MigrateHostedService> logger,
    StrackDbContext dbContext) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("正在迁移数据库");

            await dbContext.Database.MigrateAsync(cancellationToken);

            logger.LogInformation("数据库迁移完成");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "数据库迁移失败");
        }
    }
}