using Strack.Desktop.ViewModel.Shell;
using System.Windows;

namespace Strack.Desktop.UI.Shell.Main;

public partial class MainShell : Window, IBindsViewModel<MainShellViewModel>
{
    public MainShellViewModel ViewModel { get; }

    public MainShell(IServiceProvider services)
    {
        InitializeComponent();

        ViewModel = services.GetMainWindowViewModel();
        DataContext = ViewModel;
    }
}