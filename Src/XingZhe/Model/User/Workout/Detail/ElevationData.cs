using UnitsNet;

namespace XingZhe.Model.User.Workout.Detail;


/// <summary>
/// 高程数据
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
    /// 平均坡度 (百分比)
    /// </summary>
    public double? AvgGrade { get; set; }

    /// <summary>
    /// 最小平坡度 (百分比) 
    /// </summary>
    public double? MinGrade { get; set; }

    /// <summary>
    /// 最大坡度 (百分比) 
    /// </summary>
    public double? MaxGrade { get; set; }

    #endregion

    #region --距离--

    /// <summary>
    /// 下坡距离
    /// </summary>
    public Length? DownslopeDistance { get; set; } 
    /// <summary>
    /// 上坡距离
    /// </summary>
    public Length? UpslopeDistance { get; set; }
    /// <summary>
    /// 平路距离
    /// </summary>
    public Length? FlatDistance { get; set; }

    #endregion

    #region --时间--

    /// <summary>
    /// 下降时长
    /// </summary>
    public TimeSpan? DownslopeDuration { get; set; }
    /// <summary>
    /// 上升时长
    /// </summary>
    public TimeSpan? UpslopeDuration { get; set; }
    /// <summary>
    /// 平路时长
    /// </summary>
    public TimeSpan? FlatDuration { get; set; }

    #endregion

    #region --速度--

    /// <summary>
    /// 上升速度
    /// </summary>
    public Speed? AscentSpeed { get; set; }

    #endregion
}