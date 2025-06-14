using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail.Metrics;

/// <summary>
/// 踏频数据
/// </summary>
public class CadenceMetrics
{
    /// <summary>
    /// 最高踏频
    /// </summary>
    public Frequency? Max { get; set; }

    /// <summary>
    /// 平均踏频
    /// </summary>
    public Frequency? Avg { get; set; }


    public override string ToString() => $"最高:{Max}, 平均{Avg}";
}
