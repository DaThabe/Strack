using UnitsNet;
using XingZhe.Model.User.Workout.Detail.Metrics;

namespace XingZhe.Model.User.Workout.Detail;


/// <summary>
/// 训练明细
/// </summary>
public partial class WorkoutDetail
{
    /// <summary>
    /// Id
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 运动类型
    /// </summary>
    public required WorkoutType Type { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 消耗热量
    /// </summary>
    public Energy? Calories { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public required DateTimeOffset BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public required DateTimeOffset FinishTime { get; set; }


    

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
    /// <summary>
    /// 用户信息
    /// </summary>
    public required UserMetrics User { get; set; }


    /// <summary>
    /// 转为字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{BeginTime:yyyy-MM-dd HH:mm:ss}-{Title}";
}