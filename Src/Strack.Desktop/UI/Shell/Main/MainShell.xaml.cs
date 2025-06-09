using Strack.Desktop.Model.Exception;
using Strack.Desktop.ViewModel.Shell;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace Strack.Desktop.UI.Shell.Main;

public partial class MainShell : INavigableView<MainShellViewModel>
{
    public MainShellViewModel ViewModel => DataContext as MainShellViewModel ?? throw new StrackDesktopException("主窗体视图模型为空");


    public MainShell(
        ISnackbarService snackbarService,
        IContentDialogService contentDialogService,
        INavigationService navigationService,
        MainShellViewModel vm
        )
    {
        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);
        DataContext = vm;

        InitializeComponent();

        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        contentDialogService.SetDialogHost(DialogHost);
        navigationService.SetNavigationControl(NavigationControl);
    }
}