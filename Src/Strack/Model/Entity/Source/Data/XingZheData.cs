using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Source.Data;

[Table("SourceXingZhe")]
public class XingZheData : EntityBase
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 训练Id
    /// </summary>
    public required long WorkoutId { get; set; }


    /// <summary>
    /// 来源Id
    /// </summary>
    public required Guid SourceId { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    [ForeignKey(nameof(SourceId))]
    public required SourceEntity Source { get; set; }
}