using FluentFrame.ViewModel.Shell;
using FluentFrame.ViewModel.Shell.Message;
using FluentFrame.ViewModel.Shell.Notify;

namespace FluentFrame.Service.Shell;


/// <summary>
/// 窗口业务
/// </summary>
public interface IFluentShellService
{
    /// <summary>
    /// 是否开启暗色模式
    /// </summary>
    bool IsDarkTheme { get; set; }

    /// <summary>
    /// 设置vm
    /// </summary>
    /// <param name="source"></param>
    void SetSourceProvider(FluentShellViewModel source);

    /// <summary>
    /// 发送通知
    /// </summary>
    /// <returns></returns>
    Task NotifyAsync(NotifyItemViewModel item);

    /// <summary>
    /// 添加消息
    /// </summary>
    /// <param name="item"></param>
    void AddMessage(MessageItemViewModel item);
}

public class FluentShellService : IFluentShellService
{
    public void SetSourceProvider(FluentShellViewModel vm) => _source = vm;

    public bool IsDarkTheme
    {
        get => _source.IsDarkTheme;
        set => _source.IsDarkTheme = value;
    }

    public async Task NotifyAsync(NotifyItemViewModel item)
    {
        if (_source == null) return;

        //开始播放动画
        item.IsActive = true;
        _source.Notifies.Add(item);

        //等待
        await Task.Delay(item.Delay);
        item.IsActive = false;

        //播放完动画之后删除
        await Task.Delay(200);
        _source.Notifies.Remove(item);
    }

    public void AddMessage(MessageItemViewModel item)
    {
        if (_source == null) return;
        _source.Messages.Add(item);
    }

    private FluentShellViewModel _source = null!;
}