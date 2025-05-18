using Strack.Model.Entity.Sampling.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Source.Data;


[Table("SourceXingZhe")]
public class XingzheSourceDataEntity : SourceDataEntity
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 活动Id
    /// </summary>
    public long ActivityId { get; set; }
}
