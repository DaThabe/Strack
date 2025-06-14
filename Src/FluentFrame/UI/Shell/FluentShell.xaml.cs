using FluentFrame.ViewModel.Shell;
using Wpf.Ui.Appearance;

namespace FluentFrame.UI.Shell;

public partial class FluentShell
{
    public FluentShellViewModel ViewModel => (FluentShellViewModel)DataContext;

    public FluentShell(FluentShellViewModel vm)
    {
        SystemThemeWatcher.Watch(this);

        InitializeComponent();
        DataContext = vm;
    }
}
