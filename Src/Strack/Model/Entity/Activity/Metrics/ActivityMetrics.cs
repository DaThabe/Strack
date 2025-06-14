using Microsoft.EntityFrameworkCore;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 运动活动的综合指标实体，包含高度、踏频、距离、时长、心率、功率、坡度、速度、温度等多维度数据。
/// 用于汇总和存储一次运动的各项统计数据，便于分析和展示。
/// </summary>
[Owned]
public class ActivityMetrics
{
    /// <summary>
    /// 海拔相关指标，可能包含最高、最低、平均海拔等数据
    /// </summary>
    public AltitudeMetrics Altitude { get; set; } = new();

    /// <summary>
    /// 踏频指标，如平均踏频、最大踏频等。
    /// </summary>
    public CadenceMetrics Cadence { get; set; } = new();

    /// <summary>
    /// 距离相关指标，包含总距离、上坡距离、下降距离等
    /// </summary>
    public DistanceMetrics Distance { get; set; } = new();

    /// <summary>
    /// 时长相关指标，如活动总时长、上坡时长等
    /// </summary>
    public DurationMetrics Duration { get; set; } = new();

    /// <summary>
    /// 高程相关指标, 如爬升高度,下降高度
    /// </summary>
    public ElevationMetrics Elevation { get; set; } = new();

    /// <summary>
    /// 心率相关指标，包括最高、最低、平均心率
    /// </summary>
    public HeartrateMetrics Heartrate { get; set; } = new();

    /// <summary>
    /// 功率相关指标，如最大功率、平均功率等。
    /// </summary>
    public PowerMetrics Power { get; set; } = new();

    /// <summary>
    /// 坡度相关指标，包含最大坡度、平均坡度等。
    /// </summary>
    public SlopeMetrics Slope { get; set; } = new();

    /// <summary>
    /// 速度相关指标，包括最大速度、平均速度及垂直速度。
    /// </summary>
    public SpeedMetrics Speed { get; set; } = new();

    /// <summary>
    /// 天气相关指标，如最高温度、最低温度、平均温度。
    /// </summary>
    public WeatherMetrics Weather { get; set; } = new();
}
