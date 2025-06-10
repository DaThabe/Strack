using UnitsNet;

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
    public string? Title { get; set; }

    /// <summary>
    /// 消耗热量
    /// </summary>
    public Energy? Calories { get; set; }

    /// <summary>
    /// 总距离
    /// </summary>
    public Length? Distance { get; set; }

    /// <summary>
    /// 总时间
    /// </summary>
    public TimeSpan? Duration { get; set; }


    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTimeOffset? BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTimeOffset? FinishTime { get; set; }



    /// <summary>
    /// 踏频
    /// </summary>
    public CadenceData? Cadence { get; set; }

    /// <summary>
    /// 高程
    /// </summary>
    public ElevationData? Elevation { get; set; }

    /// <summary>
    /// 心率
    /// </summary>
    public HeartrateData? Heartrate { get; set; }

    /// <summary>
    /// 功率
    /// </summary>
    public PowerData? Power { get; set; }

    /// <summary>
    /// 速度
    /// </summary>
    public SpeedData? Speed { get; set; }

    /// <summary>
    /// 温度
    /// </summary>
    public TemperatureData? Temperature { get; set; }


    /// <summary>
    /// 转为字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{BeginTime:yyyy-MM-dd HH:mm:ss}-{Title}";
}