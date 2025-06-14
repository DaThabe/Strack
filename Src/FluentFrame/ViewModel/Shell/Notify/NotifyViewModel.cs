using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell.Notify;


/// <summary>
/// 通知
/// </summary>
public partial class NotifyItemViewModel : ObservableObject
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial object? Icon { get; set; }

    /// <summary>
    /// 样式
    /// </summary>
    [ObservableProperty]
    public required partial ControlAppearance Appearance { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public required partial string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }

    /// <summary>
    /// 存在时间
    /// </summary>
    [ObservableProperty]
    public required partial TimeSpan Delay { get; set; }

    /// <summary>
    /// 是否显示关闭按钮
    /// </summary>
    [ObservableProperty]
    public partial bool IsClosable { get; set; } = true;

    /// <summary>
    /// 是否活跃中
    /// </summary>
    [ObservableProperty]
    public partial bool IsActive { get; set; } = true;
}
