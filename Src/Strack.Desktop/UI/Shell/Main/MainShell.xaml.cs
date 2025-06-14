using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.ViewModel.Shell;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace Strack.Desktop.UI.Shell.Main;

public partial class MainShell
{
    public MainShellViewModel ViewModel => (MainShellViewModel)DataContext;


    public MainShell(
        ISnackbarService snackbarService,
        INavigationService navigationService,
        IContentDialogService contentDialogService,
        MainShellViewModel vm
        )
    {
        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();
        DataContext = vm;

        //snackbarService.SetSnackbarPresenter(PART_SnackbarPresenter);
        contentDialogService.SetDialogHost(PART_ContentDialogContentPresenter);
        //navigationService.SetNavigationControl(PART_NavigationView);
        this.navigationService = navigationService;
    }


    private void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
    {
 
    }

    private Orientation _layoutOrientation = Orientation.Horizontal;
    private readonly INavigationService navigationService;
}