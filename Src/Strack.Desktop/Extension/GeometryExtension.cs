using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Strack.Desktop.Extension;

internal static class GeometryExtension
{
    /// <summary>
    /// 将经纬度坐标归一化，使最小点平移至 (0,0)，并反转纬度方向（适用于 WPF 坐标系）。
    /// </summary>
    /// <param name="positions">原始经纬度坐标（纬度, 经度）</param>
    /// <returns>归一化后的点集合</returns>
    public static List<Point> NormalizeToOrigin(this IEnumerable<(double Longitude, double Latitude)> positions)
    {
        var posList = positions.ToList();
        if (posList.Count == 0) return [];

        double minLat = posList.Min(p => p.Latitude);
        double minLon = posList.Min(p => p.Longitude);
        double maxLat = posList.Max(p => p.Latitude);

        // 注意 Y 轴反向：WPF 中 Y 轴向下增长
        return [
            .. posList.Select(p => new Point(
            p.Longitude - minLon,       // X: 经度平移
            maxLat - p.Latitude         // Y: 纬度反转 + 平移
        ))
        ];
    }

    /// <summary>
    /// 将一组坐标按指定比例进行缩放。
    /// </summary>
    /// <param name="points">归一化后的点</param>
    /// <param name="scale">缩放比例</param>
    /// <returns>缩放后的点集合</returns>
    public static List<Point> ScaleBy(this IEnumerable<Point> points, double scale)
    {
        return [.. points.Select(p => new Point(p.X * scale, p.Y * scale))];
    }

    /// <summary>
    /// 将路径坐标自动缩放，使其在指定像素尺寸内最长边为 size（保持纵横比）。
    /// </summary>
    /// <param name="points">归一化后的点</param>
    /// <param name="targetSize">目标尺寸，最长边将缩放至此值</param>
    /// <returns>缩放后的点集合</returns>
    public static List<Point> ScaleToFit(this IEnumerable<Point> points, double targetSize)
    {
        double minX = points.Min(p => p.X);
        double maxX = points.Max(p => p.X);
        double minY = points.Min(p => p.Y);
        double maxY = points.Max(p => p.Y);

        double width = maxX - minX;
        double height = maxY - minY;

        if (width == 0 && height == 0) return [.. points]; // 单点或无变化

        double scale = targetSize / Math.Max(width, height);
        return points.ScaleBy(scale);
    }

    /// <summary>
    /// 删除路径中冗余的共线点，仅保留转折点和端点。
    /// </summary>
    /// <param name="tolerance">数值越小，越严格地判断“共线”</param>
    public static List<Point> RemoveColinearPoints(this IEnumerable<Point> points, double tolerance)
    {
        var result = new List<Point>();
        var list = points.ToList();

        if (list.Count < 3) return [.. list]; // 少于3点无法共线

        result.Add(list[0]);

        for (int i = 1; i < list.Count - 1; i++)
        {
            var a = list[i - 1];
            var b = list[i];
            var c = list[i + 1];

            // 向量 AB 和 BC 的叉积，如果接近0表示共线
            double cross = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);

            if (Math.Abs(cross) > tolerance) // 不共线，保留中间点
                result.Add(b);
        }

        result.Add(list[^1]); // 最后一个点
        return result;
    }

    /// <summary>
    /// 将坐标浮点数裁剪到指定小数点后位数
    /// </summary>
    public static List<Point> RoundCoordinates(this IEnumerable<Point> points, int digits)
    {
        return [.. points.Select(p => new Point(Math.Round(p.X, digits),Math.Round(p.Y, digits)))];
    }


    /// <summary>
    /// 将点序列转换为可用于 Path 的几何数据。
    /// </summary>
    /// <param name="points">点的枚举器</param>
    /// <returns>路径几何</returns>
    public static Geometry ToGeometry(this IEnumerable<Point> points)
    {
        var geometry = new StreamGeometry();
        using StreamGeometryContext context = geometry.Open();

        var enumerator =  points.GetEnumerator();

        if (enumerator.MoveNext())
        {
            Point start = enumerator.Current;

            context.BeginFigure(start, isFilled: false, isClosed: false);

            while (enumerator.MoveNext())
            {
                context.LineTo(enumerator.Current, isStroked: true, isSmoothJoin: false);
            }
        }

        geometry.Freeze();
        return geometry;
    }
}
