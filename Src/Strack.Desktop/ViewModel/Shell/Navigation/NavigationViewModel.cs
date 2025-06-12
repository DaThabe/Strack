using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Strack.Desktop.ViewModel.Shell.Navigation;


/// <summary>
/// 导航元素
/// </summary>
public partial class NavigationViewModel : ObservableObject
{
    /// <summary>
    /// 当前导航元素
    /// </summary>
    [ObservableProperty]
    public partial NavigationItemViewModel? Current { get; set; }

    /// <summary>
    /// 前进记录
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> ForwardRecords { get; set; } = [];

    /// <summary>
    /// 后退记录
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> BackRecords { get; set; } = [];
}
