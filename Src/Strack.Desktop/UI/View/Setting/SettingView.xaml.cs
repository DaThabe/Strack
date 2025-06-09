using Strack.Desktop.ViewModel.View.Setting;
using System.Windows.Controls;

namespace Strack.Desktop.UI.View.Setting;

/// <summary>
/// SettingView.xaml 的交互逻辑
/// </summary>
public partial class SettingView : UserControl
{
    public SettingView(SettingViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
