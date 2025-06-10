using UnitsNet;

namespace XingZhe.Model.User.Workout.Detail;

/// <summary>
/// 踏频数据
/// </summary>
public class CadenceData
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
