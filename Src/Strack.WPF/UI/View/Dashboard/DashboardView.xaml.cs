using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.ViewModel.View.Dashboard;
using System.Windows.Controls;

namespace Strack.Desktop.UI.View.Dashboard;


public partial class DashboardView : IBindsViewModel<DashboardViewModel>
{
    /// <summary>
    /// 视图模型
    /// </summary>
    public DashboardViewModel ViewModel { get; }

    public DashboardView(DashboardViewModel viewModel, ActivityView activity, ImportView importView, TestView testView)
    {
        InitializeComponent();

        ViewModel = viewModel;
        DataContext = viewModel;


        viewModel.LayoutItemsSource.Add(new ViewModel.View.Dashboard.Layout.LayoutItemViewModel()
        {
            Title = "活动",
            Height = 100,
            Dock = Dock.Top,
            Content = activity
        });

        viewModel.LayoutItemsSource.Add(new ViewModel.View.Dashboard.Layout.LayoutItemViewModel()
        {
            Title = "导入",
            Dock = Dock.Right,
            Content = importView
        });

        viewModel.LayoutItemsSource.Add(new ViewModel.View.Dashboard.Layout.LayoutItemViewModel()
        {
            Title = "测试",
            Dock = Dock.Bottom,
            Content = testView
        });
    }
}
