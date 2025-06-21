using Common.Model;
using CommunityToolkit.Mvvm.Input;
using FluentFrame.ViewModel.Shell.Dialog;
using FluentFrame.ViewModel.Shell.Navigation;
using System.Drawing;
using Wpf.Ui.Controls;

namespace FluentFrame.Service.Shell.Dialog;

/// <summary>
/// 弹窗服务
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// 设置资源委托
    /// </summary>
    /// <param name="sourceProvider"></param>
    public void SetSourceProvider(IDialogSourceProvider sourceProvider);

    Task<DialogViewModel> ShowAsync(Action<DialogViewModel> action);

    /// <summary>
    /// 显示弹窗
    /// </summary>
    /// <param name="item"></param>
    Task ShowAsync(DialogViewModel item);

    /// <summary>
    /// 关闭弹窗
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task CloseAsync(DialogViewModel item);
}


public class DialogService : IDialogService
{
    public async Task<DialogViewModel> ShowAsync(Action<DialogViewModel> action)
    {
        var vm = new DialogViewModel(this) { Content = null! };
        action(vm);

        await ShowAsync(vm);
        return vm;
    }
    public async Task ShowAsync(DialogViewModel item)
    {
        await _dialogLock.WaitAsync(); // 等待上一个弹窗释放

        item.IsActive = true;
        _sourceProvider.Content = item;

        _lastTcs = new TaskCompletionSource();
        //等待关闭
        await _lastTcs.Task;

        _sourceProvider.Content = null;
        item.IsActive = false;
        _dialogLock.Release();

        return;
    }
    public async Task CloseAsync(DialogViewModel item)
    {
        item.IsActive = false;
        _sourceProvider.Content = null;

        await Task.Delay(100);

        _lastTcs?.SetResult();
        return;
    }


    public void SetSourceProvider(IDialogSourceProvider sourceProvider) => _sourceProvider = sourceProvider;

    

    private IDialogSourceProvider _sourceProvider = null!;
    private TaskCompletionSource? _lastTcs;
    private readonly SemaphoreSlim _dialogLock = new(1, 1);
}


public static class DialogServiceExtension
{
    /// <summary>
    /// 确认弹窗 (包含确认和取消)
    /// </summary>
    /// <param name="service"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static Task ShowConfirmDialogAsync(this IDialogService service, object content, Func<Task> confirmAsync, Func<Task> cancelAsync, string? title = null, IconElement? icon = null)
    {
        return service.ShowAsync(x =>
        {
            x.Title = title;
            x.Icon = icon;
            x.Content = content;
            x.Footers =
            [
                new DialogButtonViewModel()
                {
                    Content = "确认",
                    ClickCommand = new AsyncRelayCommand(confirmAsync),
                    Icon = new SymbolIcon(SymbolRegular.Checkmark24)
                },
                new DialogButtonViewModel()
                {
                    Content = "取消",
                    ClickCommand = new AsyncRelayCommand(cancelAsync),
                    Icon = new SymbolIcon(SymbolRegular.Dismiss24)
                },
            ];
        });
    }

    /// <summary>
    /// 信息弹窗 (只有默认的关闭按钮)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static Task ShowInfoDialogAsync(this IDialogService service, object content, string? title = null, IconElement? icon = null)
    {
        return service.ShowAsync(x =>
        {
            x.Title = title;
            x.Icon = icon;
            x.Content = content;
        });
    }
}
