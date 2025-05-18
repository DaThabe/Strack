using Strack.Desktop.ViewModel.View;

namespace Strack.Desktop.UI.View;

/// <summary>
/// TestView.xaml 的交互逻辑
/// </summary>
public partial class TestView : IBindsViewModel<TestViewModel>
{
    public TestView(TestViewModel vm)
    {
        InitializeComponent();

        ViewModel = vm;
        DataContext = vm;
    }

    public TestViewModel ViewModel { get; }
}
