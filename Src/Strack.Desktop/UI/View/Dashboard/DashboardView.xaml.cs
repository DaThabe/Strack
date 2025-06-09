using Strack.Desktop.Model.Exception;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.ViewModel.View.Dashboard;
using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace Strack.Desktop.UI.View.Dashboard;


public partial class DashboardView : INavigableView<DashboardViewModel>
{
    /// <summary>
    /// 视图模型
    /// </summary>
    public DashboardViewModel ViewModel => DataContext as DashboardViewModel ?? throw new StrackDesktopException("主页视图模型为空");

    public DashboardView(DashboardViewModel vm, ActivityView activity, ImportView importView, SettingView testView)
    {
        InitializeComponent();

        DataContext = vm;

        vm.LayoutItemsSource.Add(new ViewModel.View.Dashboard.Layout.LayoutItemViewModel()
        {
            Title = "活动",
            Height = 100,
            Dock = Dock.Top,
            Content = activity
        });

        vm.LayoutItemsSource.Add(new ViewModel.View.Dashboard.Layout.LayoutItemViewModel()
        {
            Title = "导入",
            Dock = Dock.Right,
            Content = importView
        });

        vm.LayoutItemsSource.Add(new ViewModel.View.Dashboard.Layout.LayoutItemViewModel()
        {
            Title = "测试",
            Dock = Dock.Bottom,
            Content = testView
        });
    }
}
