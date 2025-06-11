using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Source.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Source;


/// <summary>
/// 行者来源
/// </summary>
[Table("Source")]
public class SourceEntity : EntityBase
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public required SourceType Type { get; set; }


    /// <summary>
    /// 行者
    /// </summary>
    public XingZheData? XingZhe { get; set; }

    /// <summary>
    /// 迹驰
    /// </summary>
    public IGPSportData? IGPSport { get; set; }



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
