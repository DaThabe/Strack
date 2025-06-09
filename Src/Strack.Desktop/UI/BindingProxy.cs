using System.Windows;

namespace Strack.Desktop.UI;


public class BindingProxy : Freezable
{
    // 用于绑定的数据
    public object? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(object),
            typeof(BindingProxy),
            new UIPropertyMetadata(null));

    // 必须重写，返回新实例
    protected override Freezable CreateInstanceCore()
    {
        return new BindingProxy();
    }
}