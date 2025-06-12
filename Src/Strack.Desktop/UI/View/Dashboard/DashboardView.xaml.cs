using Strack.Desktop.ViewModel.View.Dashboard;
using System.Windows;
using Wpf.Ui.Abstractions.Controls;

namespace Strack.Desktop.UI.View.Dashboard;


public partial class DashboardView : INavigableView<DashboardViewModel>
{
    /// <summary>
    /// 视图模型
    /// </summary>
    public DashboardViewModel ViewModel => (DashboardViewModel)DataContext;

    public DashboardView(DashboardViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;


        Task.Run(async () =>
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Test.IsActive = !Test.IsActive;
                });

                await Task.Delay(500);
            }
        });
    }
}
