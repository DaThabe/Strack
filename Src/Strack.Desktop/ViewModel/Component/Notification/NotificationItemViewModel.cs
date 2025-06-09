using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace Strack.Desktop.ViewModel.Component.Notification;


/// <summary>
/// 通知元素
/// </summary>
public partial class NotificationItemViewModel : ObservableObject
{
    /// <summary>
    /// 背景色
    /// </summary>
    [ObservableProperty]
    public partial Brush Background { get; set; }

    /// <summary>
    /// 前景色
    /// </summary>
    [ObservableProperty]
    public partial Brush Foreground { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial object? Icon { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }

    /// <summary>
    /// 显示时长
    /// </summary>
    [ObservableProperty]
    public partial TimeSpan Delay { get; set; }
}
