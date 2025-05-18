using Mapsui.Tiling;
using System.Windows.Controls;

namespace Strack.Desktop.UI.View.Track;

/// <summary>
/// TrackView.xaml 的交互逻辑
/// </summary>
public partial class TrackView : UserControl
{
    public TrackView()
    {
        InitializeComponent();

        var mapControl = new Mapsui.UI.Wpf.MapControl();
        mapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());

        Map.Content = mapControl;
    }
}
