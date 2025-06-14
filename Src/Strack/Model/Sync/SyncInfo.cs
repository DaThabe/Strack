namespace Strack.Model.Sync;


/// <summary>
/// 同步信息
/// </summary>
/// <typeparam name="T"></typeparam>
public class SyncInfo<T>
{
    /// <summary>
    /// 总数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 已完成的数量
    /// </summary>
    public int Completed { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 当前数据
    /// </summary>
    public T? Item { get; set; }
}
