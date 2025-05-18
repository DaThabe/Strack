using Strack.Desktop.ViewModel.Shell;
using System.Windows;

namespace Strack.Desktop.UI.Shell;

public partial class MainWindow : Window, IBindsViewModel<MainWindowViewModel>
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(IServiceProvider services)
    {
        InitializeComponent();

        ViewModel = services.GetMainWindowViewModel();
        DataContext = ViewModel;
    }
}