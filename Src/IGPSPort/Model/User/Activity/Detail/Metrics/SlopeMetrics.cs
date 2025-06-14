namespace IGPSport.Model.User.Activity.Detail.Metrics;

/// <summary>
/// 坡度
/// </summary>
public class SlopeMetrics
{
    /// <summary>
    /// 最大上坡度 (百分比)
    /// </summary>
    public double? MaxUpslope { get; set; }
    /// <summary>
    /// 平均上坡度 (百分比)
    /// </summary>
    public double? AvgUpslope { get; set; }


    /// <summary>
    /// 最大下坡度 (百分比)
    /// </summary>
    public double? MaxDownslope { get; set; }
    /// <summary>
    /// 平均下坡度 (百分比)
    /// </summary>
    public double? AvgDownslope { get; set; }
}
