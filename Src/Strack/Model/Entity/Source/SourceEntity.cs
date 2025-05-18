using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Sampling.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Source;


/// <summary>
/// 活动
/// </summary>
[Table("Source")]
public class SourceEntity : EntityBase
{
    /// <summary>
    /// 类型
    /// </summary>
    public required SourceType Type { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public required SourceDataEntity Data { get; set; }



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