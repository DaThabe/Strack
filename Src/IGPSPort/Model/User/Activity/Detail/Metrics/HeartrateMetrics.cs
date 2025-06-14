using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail.Metrics;

/// <summary>
/// 心率数据
/// </summary>
public class HeartrateMetrics
{
    /// <summary>
    /// 最低心率
    /// </summary>
    public Frequency? Min { get; set; }

    /// <summary>
    /// 最高心率
    /// </summary>
    public Frequency? Max { get; set; }

    /// <summary>
    /// 平均心率
    /// </summary>
    public Frequency? Avg { get; set; }



    public override string ToString() => $"最大:{Max}, 平均{Avg}";
}
