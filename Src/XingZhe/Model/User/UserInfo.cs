using UnitsNet;

namespace XingZhe.Model.User;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long Id { get; set; }


    /// <summary>
    /// 头像网址
    /// </summary>
    public string AvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 电话号
    /// </summary>
    public long? PhoneNumber { get; set; }


    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; } = GenderType.Unknown;

    /// <summary>
    /// 身高
    /// </summary>
    public Length Height { get; set; } = Length.Zero;

    /// <summary>
    /// 体重
    /// </summary>
    public Mass Weight { get; set; } = Mass.Zero;

    /// <summary>
    /// 生日
    /// </summary>
    public DateTimeOffset Birthday { get; set; } = DateTimeOffset.MinValue;
}