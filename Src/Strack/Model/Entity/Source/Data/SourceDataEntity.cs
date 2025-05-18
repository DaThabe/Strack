using Strack.Model.Entity.Source;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Sampling.Data;


/// <summary>
/// 来源数据基类
/// </summary>
[Table("SourceData")]
public abstract class SourceDataEntity : EntityBase
{
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