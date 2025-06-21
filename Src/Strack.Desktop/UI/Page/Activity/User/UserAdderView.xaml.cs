using Strack.Desktop.ViewModel.Page.Activity.User;
using System.Windows.Controls;

namespace Strack.Desktop.UI.Page.Activity.User;

/// <summary>
/// UserAdder.xaml 的交互逻辑
/// </summary>
public partial class UserAdderView : UserControl
{
    public UserAdderViewModel ViewModel => (UserAdderViewModel)DataContext;

    public UserAdderView(UserAdderViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
