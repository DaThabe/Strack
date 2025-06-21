using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service.Shell.Dialog;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell.Dialog;



public partial class DialogViewModel(IDialogService dialogService) : ObservableObject
{
    /// <summary>
    /// 是否活跃中
    /// </summary>
    [ObservableProperty]
    public partial bool IsActive { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconElement? Icon { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string? Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public required partial object Content { get; set; }

    /// <summary>
    /// 操作按钮
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<DialogButtonViewModel> Footers { get; set; } = [];


    /// <summary>
    /// 关闭命令
    /// </summary>
    [RelayCommand]
    private Task CloseAsync() => dialogService.CloseAsync(this);
}

public static class DialogViewModelExtension
{
    public static void AddDialogButton(this ICollection<DialogButtonViewModel> dialogButtonViewModels, Func<Task> onClick, string content, IconElement? icon = null)
    {
        dialogButtonViewModels.Add(new DialogButtonViewModel()
        {
            Icon = icon,
            Content = content,
            ClickCommand = new AsyncRelayCommand(onClick)
        });
    }

    public static void AddDialogButton(this ICollection<DialogButtonViewModel> dialogButtonViewModels, Func<Task> onClick, IconElement icon, string? content = null)
    {
        dialogButtonViewModels.Add(new DialogButtonViewModel()
        {
            Icon = icon,
            Content = content,
            ClickCommand = new AsyncRelayCommand(onClick)
        });
    }
}