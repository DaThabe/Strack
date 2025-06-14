using Microsoft.EntityFrameworkCore;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Enum;
using Strack.Model.Entity.User.Credential;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.User;


[Table("User")]
[Index(nameof(Platform), nameof(ExternalId), IsUnique = true)]
public class UserEntity : EntityBase
{
    /// <summary>
    /// 用户来源
    /// </summary>
    public required PlatformType Platform { get; set; }
    /// <summary>
    /// 外部Id
    /// </summary>
    public required long ExternalId { get; set; }


    /// <summary>
    /// 凭证
    /// </summary>
    public UserCredentialEntity? Credential { get; set; }

    /// <summary>
    /// 所有活动
    /// </summary>
    public ICollection<ActivityEntity> Activities { get; set; } = new HashSet<ActivityEntity>();
}