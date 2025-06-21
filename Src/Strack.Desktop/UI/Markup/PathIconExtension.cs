using Strack.Desktop.UI.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace Strack.Desktop.UI.Markup;

[ContentProperty("Path")]
[MarkupExtensionReturnType(typeof(PathIcon))]
public class PathIconExtension : MarkupExtension
{
    [ConstructorArgument("path")]
    public Path? Path { get; set; }


    public PathIconExtension() { }
    public PathIconExtension(Path geometry) => Path = geometry;


    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Path is null) return null;
        return new PathIcon(Path);
    }
}