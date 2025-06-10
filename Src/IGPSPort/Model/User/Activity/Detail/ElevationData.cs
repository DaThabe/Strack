using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail;


/// <summary>
/// 高程
/// </summary>
public class ElevationData
{
    #region --海拔--
    
    /// <summary>
    /// 最低海拔
    /// </summary>
    public Length? MinAltitude { get; set; }

    /// <summary>
    /// 最高海拔
    /// </summary>
    public Length? MaxAltitude { get; set; }

    /// <summary>
    /// 平均海拔
    /// </summary>
    public Length? AvgAltitude { get; set; }

    #endregion

    #region --坡度--

    /// <summary>
    /// 最大上坡度 (百分比)
    /// </summary>
    public double? MaxUpslopeGrade { get; set; }
    /// <summary>
    /// 平均上坡度 (百分比)
    /// </summary>
    public double? AvgUpslopeGrade { get; set; }


    /// <summary>
    /// 最大下坡度 (百分比)
    /// </summary>
    public double? MaxDownslopeGrade { get; set; }
    /// <summary>
    /// 平均下坡度 (百分比)
    /// </summary>
    public double? AvgDownslopeGrade { get; set; }

    #endregion

    #region --距离--

    /// <summary>
    /// 上坡距离
    /// </summary>
    public Length? UpslopeDistance { get; set; }
    /// <summary>
    /// 下坡距离
    /// </summary>
    public Length? DownslopeDistance { get; set; }

    #endregion

    #region --高度--

    /// <summary>
    /// 总升高度
    /// </summary>
    public Length? AscentHeight { get; set; }
    /// <summary>
    /// 总降高度
    /// </summary>
    public Length? DescentHeight { get; set; }

    #endregion

    #region --速度--

    /// <summary>
    /// 最快上升速度
    /// </summary>
    public Speed? MaxAscentSpeed { get; set; }
    /// <summary>
    /// 平均上升速度
    /// </summary>
    public Speed? AvgAscentSpeed { get; set; }

    
    /// <summary>
    /// 最快下降速度
    /// </summary>
    public Speed? MaxDescentSpeed { get; set; }
    /// <summary>
    /// 平均下降速度
    /// </summary>
    public Speed? AvgDescentSpeed { get; set; }

    #endregion
}
