using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace Strack.Desktop.UI.Control;

public class PathIcon : IconElement
{
    public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
        nameof(Path),
        typeof(Path),
        typeof(PathIcon),
        new PropertyMetadata(null)
    );

    public Path Path
    {
        get => (Path)GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    public PathIcon() { }
    public PathIcon(Path path) => Path = path;

    protected override UIElement InitializeChildren()
    {
        return Path;
    }
}
