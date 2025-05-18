using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Sampling.Data;


/// <summary>
/// c
/// </summary>
[Table("SamplingData")]
public abstract class SamplingDataEntity : EntityBase
{
    /// <summary>
    /// 采样Id
    /// </summary>
    public required Guid SamplingId { get; set; }

    /// <summary>
    /// 采样
    /// </summary>
    [ForeignKey(nameof(SamplingId))]
    public required SamplingEntity Sampling { get; set; }
}