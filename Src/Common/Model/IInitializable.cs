namespace Common.Model;


/// <summary>
/// 表示一个需要显示调用初始化的对象
/// </summary>
public interface IInitialize
{
    /// <summary>
    /// 调用后初始化
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync(CancellationToken cancellationToken);
}