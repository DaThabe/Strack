using Strack.Desktop.ViewModel.View.Activity;
using System.Windows.Controls;

namespace Strack.Desktop.UI.View.Activity;

public partial class ActivityPage : UserControl
{
    public ActivityPageViewModel ViewModel => (ActivityPageViewModel)DataContext;

    public ActivityPage(ActivityPageViewModel vm )
    {
        InitializeComponent();
        DataContext = vm;
    }
}
