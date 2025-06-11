using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Source.Data;

namespace Strack.Model.Database;

public class StrackDbContext(DbContextOptions<StrackDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 所有活动
    /// </summary>
    public DbSet<ActivityEntity> Activities { get; set; }

    /// <summary>
    /// 行者来源
    /// </summary>
    public DbSet<XingZheData> SourceXingZhe { get; set; }

    /// <summary>
    /// 迹驰来源
    /// </summary>
    public DbSet<IGPSportData> SourceIGSPSport { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(StrackDbContext).Assembly);
    }
}


internal class StrackDbContextDesignTimeFactory : IDesignTimeDbContextFactory<StrackDbContext>
{
    public StrackDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StrackDbContext>();
        optionsBuilder.UseSqlite("Data Source=strack.db");

        return new StrackDbContext(optionsBuilder.Options);
    }
}