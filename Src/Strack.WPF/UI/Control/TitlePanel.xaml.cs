using System.Windows;

namespace Strack.Desktop.UI.Control;

/// <summary>
/// TitlePanel.xaml 的交互逻辑
/// </summary>
public partial class TitlePanel
{
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
           nameof(Header),
           typeof(object),
           typeof(TitlePanel),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty OptionProperty = DependencyProperty.Register(
           nameof(Option),
           typeof(object),
           typeof(TitlePanel),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// 标头
    /// </summary>
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// 选项
    /// </summary>
    public object? Option
    {
        get => GetValue(OptionProperty);
        set => SetValue(OptionProperty, value);
    }


    public TitlePanel()
    {
        InitializeComponent();
    }
}
