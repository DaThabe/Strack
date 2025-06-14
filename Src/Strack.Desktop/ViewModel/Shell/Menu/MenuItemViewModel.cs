using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace Strack.Desktop.ViewModel.Shell.Menu;


/// <summary>
/// 菜单元素
/// </summary>
public partial class MenuItemViewModel : ObservableObject
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public required partial IconElement Icon { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public required partial string Title { get; set; }

    /// <summary>
    /// 目标页面类型
    /// </summary>
    [ObservableProperty]
    public required partial Type TargetPageType { get; set; }


    /// <summary>
    /// 子菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MenuItemViewModel> Childs { get; set; } = [];
}
