using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.Activity.Source;
using Strack.Model.Entity.User;
using Strack.Model.Entity.User.Credential;


namespace Strack.Data;

public class StrackDbContext(DbContextOptions<StrackDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 活动
    /// </summary>
    public DbSet<ActivityEntity> Activities { get; set; }
    /// <summary>
    /// 活动来源
    /// </summary>
    public DbSet<ActivitySourceEntity> ActivitySources { get; set; }
    /// <summary>
    /// 活动记录
    /// </summary>
    public DbSet<ActivityRecordEntity> ActivityRecords { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }
    /// <summary>
    /// 用户凭证
    /// </summary>
    public DbSet<UserCredentialEntity> UserCredentials { get; set; }

    
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