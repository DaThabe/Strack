using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace Strack.Desktop.ViewModel.Shell.Navigation.Item;


/// <summary>
/// 导航到页面元素视图模型
/// </summary>
public partial class NavigationItemViewModel : ObservableObject
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial Geometry Icon { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; }

    /// <summary>
    /// 是否选中元素
    /// </summary>
    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    /// <summary>
    /// 目标页面类型
    /// </summary>
    [ObservableProperty]
    public partial Type TargetPageType { get; set; }
}
