using Microsoft.EntityFrameworkCore;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.User.Credential;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.User;


[Table("User")]
[Index(nameof(Source), nameof(ExternalId), IsUnique = true)]
public class UserEntity : EntityBase
{
    /// <summary>
    /// 用户来源
    /// </summary>
    public required SourceType Source { get; set; }
    /// <summary>
    /// 外部Id
    /// </summary>
    public required long ExternalId { get; set; }


    /// <summary>
    /// 凭证Id
    /// </summary>
    public required Guid? CredentialId { get; set; }
    /// <summary>
    /// 凭证
    /// </summary>
    [ForeignKey(nameof(CredentialId))]
    public CredentialEntity Credential { get; set; } = null!;


    /// <summary>
    /// 所有活动
    /// </summary>
    public ICollection<ActivityEntity> Activities { get; set; } = new HashSet<ActivityEntity>();
}
