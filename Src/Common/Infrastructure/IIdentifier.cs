namespace Common.Infrastructure;


/// <summary>
/// 表示具有唯一标识
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIdentifier<T> where T : notnull
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    T Id { get; }
}
