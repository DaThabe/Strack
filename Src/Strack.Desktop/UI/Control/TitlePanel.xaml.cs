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

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
           nameof(CornerRadius),
           typeof(CornerRadius),
           typeof(TitlePanel),
           new FrameworkPropertyMetadata(new CornerRadius(10), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(
           nameof(ContentMargin),
           typeof(Thickness),
           typeof(TitlePanel),
           new FrameworkPropertyMetadata(new Thickness(5), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty TitleHeightProperty = DependencyProperty.Register(
           nameof(TitleHeight),
           typeof(double),
           typeof(TitlePanel),
           new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


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

    /// <summary>
    /// 圆角
    /// </summary>
    public CornerRadius? CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// 内容边距
    /// </summary>
    public Thickness? ContentMargin
    {
        get => (Thickness)GetValue(ContentMarginProperty);
        set => SetValue(ContentMarginProperty, value);
    }


    /// <summary>
    /// 标题高度
    /// </summary>
    public double TitleHeight
    {
        get => (double)GetValue(TitleHeightProperty);
        set => SetValue(TitleHeightProperty, value);
    }


    public TitlePanel()
    {
        InitializeComponent();
    }
}
