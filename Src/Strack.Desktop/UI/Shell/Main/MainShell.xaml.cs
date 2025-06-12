using Strack.Desktop.ViewModel.Shell;
using Wpf.Ui;

namespace Strack.Desktop.UI.Shell.Main;

public partial class MainShell
{
    public MainShellViewModel ViewModel => (MainShellViewModel)DataContext;


    public MainShell(
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService,
        MainShellViewModel vm
        )
    {
        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();
        DataContext = vm;

        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetDialogHost(DialogHost);
        //navigationService.SetNavigationControl(NavigationControl);
    }
}