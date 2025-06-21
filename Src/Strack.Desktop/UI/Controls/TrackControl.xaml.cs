using Common.Model.File.Gpx;
using Common.Service.File;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Extension;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Strack.Desktop.UI.Controls;

public partial class TrackControl : UserControl
{
    /// <summary>
    /// 轨迹点
    /// </summary>
    public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
        nameof(Points),
        typeof(IEnumerable<(double Longitude, double Latitude)>),
        typeof(TrackControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPointsPropertyChanged));

    /// <summary>
    /// 轨迹线条粗度
    /// </summary>
    public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
        nameof(LineThickness),
        typeof(double),
        typeof(TrackControl),
        new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// 轨迹线条粗度
    /// </summary>
    public double LineThickness
    {
        get => (double)GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    /// <summary>
    /// 轨迹点
    /// </summary>
    public IEnumerable<(double Longitude, double Latitude)> Points
    {
        get => (IEnumerable<(double Longitude, double Latitude)>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public TrackControl()
    {
        InitializeComponent();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        if (PART_Path.Data is { } geometry && geometry.Bounds.Width > 0 && geometry.Bounds.Height > 0)
        {
            double sx = ActualWidth / geometry.Bounds.Width;
            double sy = ActualHeight / geometry.Bounds.Height;

            PART_Path.StrokeThickness = LineThickness * Math.Max(sx, sy);
        }
    }


    private static void OnPointsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        _ = Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (sender is not TrackControl control) return;
            if (e.NewValue is not IEnumerable<(double Longitude, double Latitude)> positionPoints) return;

            var points = positionPoints
                .NormalizeToOrigin()
                .ScaleToFit(800)
                .RoundCoordinates(2);

            control.PART_Path.Data = points.ToGeometry();
            
        },  DispatcherPriority.Background);
    }
}
