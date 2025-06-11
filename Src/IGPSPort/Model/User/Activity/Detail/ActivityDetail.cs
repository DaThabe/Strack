using UnitsNet;

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
    /// 总距离
    /// </summary>
    public Length? Distance { get; set; }

    /// <summary>
    /// 总时间
    /// </summary>
    public TimeSpan Duration { get; set; }


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
}
