using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace Strack.Desktop.ViewModel.View.Dashboard.Layout;


/// <summary>
/// 对齐内容
/// </summary>
public partial class LayoutItemViewModel : ObservableObject
{
    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public required partial string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public required partial object Content { get; set; }


    /// <summary>
    /// 对齐方式
    /// </summary>
    [ObservableProperty]
    public partial Dock? Dock { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    [ObservableProperty]
    public partial double Width { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    [ObservableProperty]
    public partial double Height { get; set; }
}
