using Strack.Desktop.ViewModel.View.Dashboard;
using System.Windows.Controls;

namespace Strack.Desktop.UI.View.Dashboard;


public partial class DashboardView : UserControl, IBindsViewModel<DashboardViewModel>
{
    public DashboardViewModel ViewModel { get; }

    public DashboardView(DashboardViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        DataContext = viewModel;
    }
}
