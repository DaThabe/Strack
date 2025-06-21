namespace Common.Infrastructure;

public class CancellableOperation
{
    /// <summary>
    /// 取消令牌
    /// </summary>
    public CancellationToken Token => _cts?.Token ?? CancellationToken.None;

    /// <summary>
    /// 取消上一个操作并创建新的取消令牌
    /// </summary>
    public async Task CancelPrevious()
    {
        if (_cts == null) return;

        await _cts.CancelAsync();
        _cts.Dispose();
    }

    /// <summary>
    /// 取消上一个操作, 开始新的操作并返回取消令牌
    /// </summary>
    /// <returns></returns>
    public async Task<CancellationToken> StartNew()
    {
        await CancelPrevious();

        _cts = new();
        return _cts.Token;
    }

    /// <summary>
    /// 取消当前操作
    /// </summary>
    /// <returns></returns>
    public async Task CancelAsync()
    {
        if (_cts == null) return;
        await _cts.CancelAsync();
    }



    private CancellationTokenSource? _cts;
}