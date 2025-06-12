using System.Windows.Controls;
using Wpf.Ui;

namespace Strack.Desktop.UI.View.Dashboard.User;

/// <summary>
/// AddUserView.xaml 的交互逻辑
/// </summary>
public partial class AddUserView : UserControl
{
    public AddUserView(INavigationService navigationService)
    {
        InitializeComponent();
        navigationService.SetNavigationControl(NavigationView);
    }
}
