using BruTile.Predefined;
using BruTile.Web;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Fetcher;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using Mapsui.UI.Wpf;
using Mapsui.Widgets;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using Strack.Service.External.XingZhe;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Strack.Desktop.UI.View.Track;

/// <summary>
/// TrackView.xaml 的交互逻辑
/// </summary>
public partial class TrackView : UserControl
{
    public Map Map => MapControl.Map;

    public TrackView(IXingZheApi xingzheApi)
    {
        InitializeComponent();
        LightStyle();
        this.xingzheApi = xingzheApi;
    }

    public void DarkStyle()
    {
        Map.Layers.Add(_darkLayer);
        Map.Layers.Remove(_lightLayer);
    }

    public void LightStyle()
    {
        Map.Layers.Add(_lightLayer);
        Map.Layers.Remove(_darkLayer);
    }


    private static readonly Hyperlink _layerAttribution = new Hyperlink()
    {
        Text = "© OpenStreetMap contributors © CARTO",
        TextColor = new Mapsui.Styles.Color(0, 173, 231),
        Url = "https://carto.com/attributions"
    };

    private static readonly TileLayer _darkLayer = new(new HttpTileSource(new GlobalSphericalMercator(), _darkLayerUrlFormat, ["a", "b", "c", "d"]))
    {
        Attribution = _layerAttribution
    };

    private static readonly TileLayer _lightLayer = new(new HttpTileSource(new GlobalSphericalMercator(), _lightLayerUrlFormat, ["a", "b", "c", "d"]))
    {
        Attribution = _layerAttribution
    };
    private const string _darkLayerUrlFormat = "https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}.png";
    
    private const string _lightLayerUrlFormat = "https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png";
    private readonly IXingZheApi xingzheApi;

    private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var all =  await xingzheApi.GetActivitySummariesAsync(6117373);

        for (int i = 0; i < 30; i++)
        {
            var point = await xingzheApi.GetTrackPointSummaryAsync(6117373, all[i].Id);

            CreatePathLayer(point.Select(x => (x.Longitude, x.Latitude));
        }
    }

    public static ILayer? CreatePathLayer(IEnumerable<(double lon, double lat)> gpsPoints)
    {
        var projectedPoints = gpsPoints
            .Select(p => SphericalMercator.FromLonLat(p.lon, p.lat))
            .Select(p => new Coordinate(p.x, p.y))
            .ToArray();

        if (projectedPoints.Length < 2) return null;

        var lineString = new LineString(projectedPoints);
        var feature = new Feature(lineString, new AttributesTable());

        return new MemoryLayer
        {
            Name = "路径轨迹",
            Style = new VectorStyle
            {
                Line = new Pen { Color = Color.Red, Width = 3 }
            },
            Features = [feature]             
        };
    }
}