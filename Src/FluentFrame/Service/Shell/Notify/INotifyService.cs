using FluentFrame.ViewModel.Shell.Notify;
using Wpf.Ui.Controls;

namespace FluentFrame.Service.Shell.Notify;

/// <summary>
/// 通知服务
/// </summary>
public interface INotifyService
{
    /// <summary>
    /// 设置资源委托
    /// </summary>
    /// <param name="sourceProvider"></param>
     void SetSourceProvider(INotifySourceProvider sourceProvider);

    /// <summary>
    /// 通知
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task ShowAsync(NotifyItemViewModel item, CancellationToken cancellation = default);

    /// <summary>
    /// 关闭通知
    /// </summary>
    /// <param name="item"></param>
    Task CloseAsync(NotifyItemViewModel item, CancellationToken cancellation = default);
}

public class NotifyService : INotifyService
{
    public void SetSourceProvider(INotifySourceProvider sourceProvider) => _sourceProvider = sourceProvider;


    public async Task ShowAsync(NotifyItemViewModel item, CancellationToken cancellation = default)
    {
        //开始播放动画
        item.IsActive = true;
        _sourceProvider.ItemsSource.Add(item);

        //等待
        await Task.Delay(item.Delay, cancellation);
        item.IsActive = false;

        //播放完动画之后删除
        await Task.Delay(200, cancellation);
        _sourceProvider.ItemsSource.Remove(item);
    }
    public async Task CloseAsync(NotifyItemViewModel item, CancellationToken cancellation = default)
    {
        item.IsActive = false;

        await Task.Delay(1000, cancellation);
        _sourceProvider.ItemsSource.Remove(item);
    }

   
    private INotifySourceProvider _sourceProvider = null!;
}


public static class NotifyServiceExtension
{
    public static Task ShowErrorAsync(this INotifyService service, string content, string title = "错误", double delaySeconds = 2, CancellationToken cancellation = default)
    {
        return service.ShowAsync(new()
        {
            Appearance = ControlAppearance.Danger,
            Delay = TimeSpan.FromSeconds(delaySeconds),
            Title = title,
            Content = content,
            Icon = new SymbolIcon(SymbolRegular.Dismiss24),
            IsClosable = false

        }, cancellation);
    }

    public static Task ShowWarningAsync(this INotifyService service, string content, string title = "警告", double delaySeconds = 1.5, CancellationToken cancellation = default)
    {
        return service.ShowAsync(new()
        {
            Appearance = ControlAppearance.Caution,
            Delay = TimeSpan.FromSeconds(delaySeconds),
            Title = title,
            Content = content,
            Icon = new SymbolIcon(SymbolRegular.Warning24),
            IsClosable = false

        }, cancellation);
    }

    public static Task ShowInfoAsync(this INotifyService service, string content, string title = "提示", double delaySeconds = 1, CancellationToken cancellation = default)
    {
        return service.ShowAsync(new()
        {
            Appearance = ControlAppearance.Info,
            Delay = TimeSpan.FromSeconds(delaySeconds),
            Title = title,
            Content = content,
            Icon = new SymbolIcon(SymbolRegular.Alert24),
            IsClosable = false

        }, cancellation);
    }

    public static Task ShowSuccessAsync(this INotifyService service, string content, string title = "成功", double delaySeconds = 1, CancellationToken cancellation = default)
    {
        return service.ShowAsync(new()
        {
            Appearance = ControlAppearance.Success,
            Delay = TimeSpan.FromSeconds(delaySeconds),
            Title = title,
            Content = content,
            Icon = new SymbolIcon(SymbolRegular.Checkmark24),
            IsClosable = false

        }, cancellation);
    }
}