using FluentFrame.ViewModel.View;
using System.Windows.Controls;

namespace FluentFrame.UI.View;

/// <summary>
/// DemoView.xaml 的交互逻辑
/// </summary>
public partial class DemoView : UserControl
{
    public DemoView(DemoViewModel demoViewModel)
    {
        InitializeComponent();
        DataContext = demoViewModel;
    }
}
