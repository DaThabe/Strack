namespace Common.Model.File.Tcx;

/// <summary>
/// 活动
/// </summary>
public class Activity
{
    /// <summary>
    /// 活动类型
    /// </summary>
    public SportType Sport { get; set; } = SportType.Other;

    /// <summary>
    /// Id (该活动中具有唯一性的时间)
    /// </summary>
    public DateTimeOffset Id { get; set; }  = DateTimeOffset.MinValue;
}

public class Lap
{
    /// <summary>
    /// 总时间 (秒)
    /// </summary>
    public int TotalTimeSeconds { get; set; }

    /// <summary>
    /// 总距离 (米)
    /// </summary>
    public double DistanceMeters { get; set; }

    /// <summary>
    /// 消耗卡路里 (卡路里)
    /// </summary>
    public int Calories { get; set; }

    /// <summary>
    /// 运动强度
    /// </summary>
    public IntensityType Intensity { get; set; }

    /// <summary>
    /// 出发类型
    /// </summary>
    public TriggerType Trigger { get; set; }
}

/// <summary>
/// 表示该段运动的强度类型，来自 TCX 标准中的 Intensity_t。
/// </summary>
public enum IntensityType
{
    /// <summary>
    /// 正常的运动强度，例如正式训练或比赛。
    /// </summary>
    Active,

    /// <summary>
    /// 休息状态，例如中途暂停或静止不动。
    /// </summary>
    Resting,

    /// <summary>
    /// 热身阶段，通常在主训练开始之前。
    /// </summary>
    Warmup,

    /// <summary>
    /// 放松阶段，通常在主训练结束之后。
    /// </summary>
    Cooldown
}

/// <summary>
/// 表示圈（Lap）是如何触发记录的，来自 TCX 标准中的 TriggerMethod_t。
/// </summary>
public enum TriggerType
{
    /// <summary>
    /// 手动触发，例如用户按下“圈”按钮。
    /// </summary>
    Manual,

    /// <summary>
    /// 根据距离自动触发，例如每 1 公里记录一次。
    /// </summary>
    Distance,

    /// <summary>
    /// 根据地理位置触发，例如到达某个坐标点。
    /// </summary>
    Location,

    /// <summary>
    /// 根据时间间隔自动触发，例如每 5 分钟记录一次。
    /// </summary>
    Time,

    /// <summary>
    /// 根据心率触发，例如当达到目标心率区间时自动记录。
    /// </summary>
    HeartRate
}