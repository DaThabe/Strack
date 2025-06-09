using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Strack.Desktop.UI.Control;

public partial class DockSelector : UserControl
{
    public static readonly DependencyProperty DockProperty = DependencyProperty.Register(
        nameof(Dock),
        typeof(Dock),
        typeof(DockSelector),
        new FrameworkPropertyMetadata(Dock.Top, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) =>
        {
            if(sender is not DockSelector dockSelector) return;
            var dock = (Dock)e.NewValue;
            Update(dockSelector, dock);
        }));

    public Dock Dock
    {
        get => (Dock)GetValue(DockProperty);
        set => SetValue(DockProperty, value);
    }


    public DockSelector()
    {
        InitializeComponent();
        Update(this, Dock);
    }


    private static void Update(DockSelector dockSelector, Dock dock)
    {
        dockSelector.Left.Fill = dockSelector.Background;
        dockSelector.Top.Fill = dockSelector.Background;
        dockSelector.Right.Fill = dockSelector.Background;
        dockSelector.Bottom.Fill = dockSelector.Background;

        if (dock == Dock.Left) dockSelector.Left.Fill = dockSelector.Foreground;
        else if (dock == Dock.Top) dockSelector.Top.Fill = dockSelector.Foreground;
        else if (dock == Dock.Right) dockSelector.Right.Fill = dockSelector.Foreground;
        else if (dock == Dock.Bottom) dockSelector.Bottom.Fill = dockSelector.Foreground;
    }

    private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (sender is not Path path) return;

        path.Stroke = this.Foreground;
        path.StrokeThickness = 1.0;
    }

    private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (sender is not Path path) return;

        path.Stroke = this.Foreground;
        path.StrokeThickness = 1.0;
    }
}
