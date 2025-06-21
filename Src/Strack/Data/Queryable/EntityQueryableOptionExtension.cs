namespace Strack.Data.Queryable;


public static class EntityQueryableOptionExtension
{
    public static IQueryable<TEntity> Option<TEntity>(this IQueryable<TEntity> queryable, EntityQueryableOptionHandler<TEntity>? option = null)
    {
        return option?.Invoke(queryable) ?? queryable;
    }
}
