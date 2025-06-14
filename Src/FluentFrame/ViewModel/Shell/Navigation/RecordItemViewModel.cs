using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell.Navigation;


/// <summary>
/// 导航记录
/// </summary>
public partial class RecordItemViewModel : ObservableObject
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconElement? Icon { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public required partial object Content { get; set; }

    /// <summary>
    /// 目标页面类型
    /// </summary>
    [ObservableProperty]
    public required partial Type TargetPageType { get; set; }

    /// <summary>
    /// 悬浮提示
    /// </summary>
    [ObservableProperty]
    public partial string? Tooltip { get; set; }
}
