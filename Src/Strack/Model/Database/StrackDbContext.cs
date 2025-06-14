using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Source.Data;
using Strack.Model.Entity.User;
using Strack.Model.Entity.User.Activity.Source;
using Strack.Model.Entity.User.Data;

namespace Strack.Model.Database;

public class StrackDbContext(DbContextOptions<StrackDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 活动
    /// </summary>
    public DbSet<ActivityEntity> Activities { get; set; }
    /// <summary>
    /// 用户
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }
    /// <summary>
    /// 来源
    /// </summary>
    public DbSet<CredentialEntity> Sources { get; set; }


    /// <summary>
    /// 行者来源
    /// </summary>
    public DbSet<XingZheSourceEntity> XingZheSources { get; set; }
    /// <summary>
    /// 迹驰来源
    /// </summary>
    public DbSet<IGPSportSourceEntity> IGPSportSources { get; set; }


    /// <summary>
    /// 行者用户
    /// </summary>
    public DbSet<XingZheUserEntity> XingzheUsers { get; set; }
    /// <summary>
    /// 迹驰用户
    /// </summary>
    public DbSet<IGPSportUserEntity> IGPSportUsers { get; set; }

    
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