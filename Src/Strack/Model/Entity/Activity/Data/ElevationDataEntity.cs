using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Data;

/// <summary>
/// 高程数据
/// </summary>
[Table("ActivityElevation")]
public class ElevationDataEntity : EntityBase
{
    #region --海拔--

    /// <summary>
    /// 平均海拔 (米)
    /// </summary>
    public double? AvgAltitudeMeters { get; set; }

    /// <summary>
    /// 最低海拔 (米)
    /// </summary>
    public double? MinAltitudeMeters { get; set; }

    /// <summary>
    /// 最高海拔 (米)
    /// </summary>
    public double? MaxAltitudeMeters { get; set; }

    #endregion

    #region --坡度--

    /// <summary>
    /// 平均坡度
    /// </summary>
    public double? AvgGrade { get; set; }
    /// <summary>
    /// 最小坡度
    /// </summary>
    public double? MinGrade { get; set; }
    /// <summary>
    /// 最大坡度
    /// </summary>
    public double? MaxGrade { get; set; }

    

    /// <summary>
    /// 平均上坡度 (百分比)
    /// </summary>
    public double? AvgUpslopeGrade { get; set; }
    /// <summary>
    /// 平均下坡度 (百分比)
    /// </summary>
    public double? AvgDownslopeGrade { get; set; }


    /// <summary>
    /// 最大上坡度 (百分比)
    /// </summary>
    public double? MaxUpslopeGrade { get; set; }
    /// <summary>
    /// 最大下坡度 (百分比)
    /// </summary>
    public double? MaxDownslopeGrade { get; set; }

    #endregion

    #region --距离--

    /// <summary>
    /// 下降距离 (米)
    /// </summary>
    public double? DownslopeDistanceMeters { get; set; }
    /// <summary>
    /// 上升距离  (米)
    /// </summary>
    public double? UpslopeDistanceMeters { get; set; } 
    /// <summary>
    /// 平路距离  (米)
    /// </summary>
    public double? FlatDistanceMeters { get; set; }

    #endregion

    #region --高度--

    /// <summary>
    /// 下降高度 (米)
    /// </summary>
    public double? DescentHeightMeters { get; set; }
    /// <summary>
    /// 上升高度  (米)
    /// </summary>
    public double? AscentHeightMeters { get; set; }

    #endregion

    #region --速度--

    /// <summary>
    /// 平均上升速度  (米/时)
    /// </summary>
    public double? AvgAscentSpeed { get; set; }
    /// <summary>
    /// 最快上升速度 (米/时)
    /// </summary>
    public double? MaxAscentSpeed { get; set; }

    /// <summary>
    ///  平均下降速度 (米/时)
    /// </summary>
    public double? AvgDescentSpeed { get; set; }
    /// <summary>
    /// 最快下降速度 (米/时)
    /// </summary>
    public double? MaxDescentSpeed { get; set; }

    #endregion

    #region --时间--

    /// <summary>
    /// 下坡时长 (秒)
    /// </summary>
    public double? DownslopeDurationSeconds { get; set; }
    /// <summary>
    /// 上坡时长  (秒)
    /// </summary>
    public double? UpslopeDurationSeconds { get; set; }
    /// <summary>
    /// 平路时长  (秒)
    /// </summary>
    public double? FlatDurationSeconds { get; set; }

    #endregion

    /// <summary>
    /// 活动Id
    /// </summary>
    public required Guid ActivityId { get; set; }

    /// <summary>
    /// 活动
    /// </summary>
    [ForeignKey(nameof(ActivityId))]
    public required ActivityEntity Activity { get; set; }
}