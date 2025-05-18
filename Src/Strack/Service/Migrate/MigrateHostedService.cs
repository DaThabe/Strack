using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Model.Database;

namespace Strack.Service.Migrate;

internal class MigrateHostedService(
    ILogger<MigrateHostedService> logger,
    StrackDbContext dbContext) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "数据库迁移失败");
        }
    }
}