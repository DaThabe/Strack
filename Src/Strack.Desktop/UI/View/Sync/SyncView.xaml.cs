using Strack.Desktop.ViewModel.View.Sync;
using System.Windows.Controls;

namespace Strack.Desktop.UI.View.Sync;

/// <summary>
/// SyncView.xaml 的交互逻辑
/// </summary>
public partial class SyncView : UserControl
{
    public SyncViewModel ViewModel => (SyncViewModel)DataContext;

    public SyncView(SyncViewModel vm )
    {
        InitializeComponent();
        DataContext = vm;


        vm.Users.Add(new Desktop.ViewModel.View.Sync.User.UserItemViewModel() { Name = "测试", SyncCommand = null });
        vm.Users.Add(new Desktop.ViewModel.View.Sync.User.UserItemViewModel() { Name = "测试2", SyncCommand = null });


        vm.Activities.Add(new() { Title = "测试1", Time = DateTime.Now, SyncCommand = null });
    }
}
