using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Strack.Desktop.ViewModel.Shell.Navigation;


/// <summary>
/// 导航视图数据
/// </summary>
public partial class NavigationViewModel : ObservableObject
{
    /// <summary>
    /// 宽度
    /// </summary>
    [ObservableProperty] 
    public partial double Width { get; private set; }

    /// <summary>
    /// 高度
    /// </summary>
    [ObservableProperty] 
    public partial double Height { get; private set; }

    /// <summary>
    /// 尺寸
    /// </summary>
    [ObservableProperty]
    public partial double Size { get; set; } = 80;


    /// <summary>
    /// 方向
    /// </summary>
    [ObservableProperty]
    public partial Orientation Orientation { get; set; } = Orientation.Vertical;


    /// <summary>
    /// 导航
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> ItemsSource { get; set; } = [];

    /// <summary>
    /// 页脚导航
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> FooterItemsSource { get; set; } = [];




    partial void OnSizeChanged(double value)
    {
        if (value < 0) return;

        if(Orientation == Orientation.Horizontal)
        {
            Height = value;
            Width = double.NaN;
        }
        else
        {
            Width = value;
            Height = double.NaN;
        }
    }
    partial void OnOrientationChanged(Orientation value)
    {
        OnSizeChanged(Size);
    }


    [RelayCommand]
    public void OnNavigation(NavigationItemViewModel item)
    {

    }
}
