using Microsoft.EntityFrameworkCore;
using Strack.Data;

namespace Strack.Service;

public interface IStrackDbService
{
    Task ExecuteAsync(Func<StrackDbContext, Task> action, CancellationToken cancellation = default);
    Task BeginTransactionAsync(Func<StrackDbContext, Task> action, CancellationToken cancellation = default);

    Task<T> ExecuteAsync<T>(Func<StrackDbContext, Task<T>> action, CancellationToken cancellation = default);
    Task<T> BeginTransactionAsync<T>(Func<StrackDbContext, Task<T>> action, CancellationToken cancellation = default);
}

public class StrackDbService(IDbContextFactory<StrackDbContext> dbFactory) : IStrackDbService
{
    public async Task BeginTransactionAsync(Func<StrackDbContext, Task> action, CancellationToken cancellation = default)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync(cancellation);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellation);

        await action(dbContext);
        await transaction.CommitAsync(cancellation);
    }

    public async Task ExecuteAsync(Func<StrackDbContext, Task> action, CancellationToken cancellation = default)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync(cancellation);
        await action(dbContext);
    }

    public  async Task<T> BeginTransactionAsync<T>(Func<StrackDbContext, Task<T>> action, CancellationToken cancellation = default)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync(cancellation);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellation);

        var result = await action(dbContext);
        await transaction.CommitAsync(cancellation);

        return result;
    }
    public async Task<T> ExecuteAsync<T>(Func<StrackDbContext, Task<T>> action, CancellationToken cancellation = default)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync(cancellation);
        return await action(dbContext);
    }
}