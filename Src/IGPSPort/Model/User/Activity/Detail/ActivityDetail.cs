using IGPSport.Model.User.Activity.Detail.Metrics;
using UnitsNet;
using XingZhe.Model.User.Workout.Detail.Metrics;

namespace IGPSport.Model.User.Activity.Detail;


/// <summary>
/// 活动明细
/// </summary>
public class ActivityDetail
{
    /// <summary>
    /// 活动Id
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public required long UserId { get; set; }

    /// <summary>
    /// Fit文件网址
    /// </summary>
    public required string FitUrl { get; set; }

    /// <summary>
    /// 活动类型
    /// </summary>
    public ActivityType Type { get; set; }

    /// <summary>
    /// 消耗热量
    /// </summary>
    public Energy? Calories { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTimeOffset BeginTime { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTimeOffset FinishTime { get; set; }
    


    /// <summary>
    /// 海拔
    /// </summary>
    public AltitudeMetrics? Altitude { get; set; }
    /// <summary>
    /// 踏频
    /// </summary>
    public CadenceMetrics? Cadence { get; set; }
    /// <summary>
    /// 距离
    /// </summary>
    public DistanceMetrics? Distance { get; set; }
    /// <summary>
    /// 时间
    /// </summary>
    public DurationMetrics? Duration { get; set; }
    /// <summary>
    /// 高程
    /// </summary>
    public ElevationMetrics? Elevation { get; set; }
    /// <summary>
    /// 心率
    /// </summary>
    public HeartrateMetrics? Heartrate { get; set; }
    /// <summary>
    /// 功率
    /// </summary>
    public PowerMetrics? Power { get; set; }
    /// <summary>
    /// 坡度
    /// </summary>
    public SlopeMetrics? Slope { get; set; }
    /// <summary>
    /// 速度
    /// </summary>
    public SpeedMetrics? Speed { get; set; }
    /// <summary>
    /// 温度
    /// </summary>
    public TemperatureMetrics? Temperature { get; set; }
}