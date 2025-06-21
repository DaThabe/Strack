using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell.Dialog;

/// <summary>
/// 弹窗按钮
/// </summary>
public partial class DialogButtonViewModel : ObservableObject
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconElement? Icon { get; set; }

    /// <summary>
    /// 文本
    /// </summary>
    [ObservableProperty]
    public partial string? Content { get; set; }

    /// <summary>
    /// 点击命令
    /// </summary>
    public required IAsyncRelayCommand ClickCommand { get; set; }
}