using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Strack.Model.Entity.Activity;

namespace Strack.Model.Database;

public class StrackDbContext(DbContextOptions<StrackDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 所有活动
    /// </summary>
    public DbSet<ActivityEntity> Activities { get; set; }



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