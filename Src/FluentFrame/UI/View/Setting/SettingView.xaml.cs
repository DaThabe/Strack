using FluentFrame.ViewModel.View.Setting;
using System.Windows.Controls;

namespace FluentFrame.UI.View.Setting;

public partial class SettingView : UserControl
{
    public SettingViewModel? ViewModel => DataContext as SettingViewModel;

    public SettingView(SettingViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
