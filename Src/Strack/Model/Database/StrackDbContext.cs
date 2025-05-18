using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Data.Cycling;
using Strack.Model.Entity.Sampling;
using Strack.Model.Entity.Sampling.Data;
using Strack.Model.Entity.Source;
using Strack.Model.Entity.Source.Data;

namespace Strack.Model.Database;

public class StrackDbContext(DbContextOptions<StrackDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 所有活动
    /// </summary>
    public DbSet<ActivityEntity> Activities { get; set; }

    /// <summary>
    /// 骑行活动扩展数据
    /// </summary>
    public DbSet<CyclingActivityDataEntity> CyclingActivityData { get; set; }


    /// <summary>
    /// 所有活动采样点
    /// </summary>
    public DbSet<SamplingEntity> Samplings { get; set; }

    /// <summary>
    /// 骑行活动采样点扩展数据
    /// </summary>
    public DbSet<CyclingSamplingDataEntity>  CyclingSamplingData { get; set; }


    /// <summary>
    /// 所有活动来源
    /// </summary>
    public DbSet<SourceEntity> Sources { get; set; }

    /// <summary>
    /// 行者来源扩展数据
    /// </summary>
    public DbSet<XingzheSourceDataEntity> XingzheSourceData { get; set; }



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