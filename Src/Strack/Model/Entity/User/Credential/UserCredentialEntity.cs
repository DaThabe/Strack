using Strack.Model.Entity.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.User.Credential;


/// <summary>
/// 活动来源
/// </summary>
[Table("UserCredential")]
public class UserCredentialEntity : EntityBase
{
    /// <summary>
    /// 凭证类型
    /// </summary>
    public required CredentialType Type { get; set; }
    /// <summary>
    /// 凭证内容
    /// </summary>
    public required string Content { get; set; }


    /// <summary>
    /// 用户Id
    /// </summary>
    public required Guid UserId { get; set; }
    /// <summary>
    /// 用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; } = null!;
}
