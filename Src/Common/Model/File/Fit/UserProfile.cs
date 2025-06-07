using Dynastream.Fit;
using UnitsNet;

namespace Common.Model.File.Fit;

/// <summary>
/// 用户信息
/// </summary>
public class UserProfile
{
    /// <summary>
    /// 用户自定义名称
    /// </summary>
    public string? FriendlyName { get; set; }

    /// <summary>
    /// 性别（Male/Female/Unspecified）
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// 年龄（岁）
    /// </summary>
    public byte? Age { get; set; }

    /// <summary>
    /// 体重
    /// </summary>
    public Mass? Weight { get; set; }

    /// <summary>
    /// 身高
    /// </summary>
    public Length? Height { get; set; }

    /// <summary>
    /// 静息心率
    /// </summary>
    public Frequency? RestingHeartRate { get; set; }

    /// <summary>
    /// 跑步最大心率
    /// </summary>
    public Frequency? MaxRunningHeartRate { get; set; }

    /// <summary>
    /// 骑行最大心率
    /// </summary>
    public Frequency? MaxBikingHeartRate { get; set; }

    /// <summary>
    /// 默认最大心率
    /// </summary>
    public Frequency? MHR { get; set; }

    /// <summary>
    /// 乳酸阈值心率
    /// </summary>
    public Frequency? LTHR { get; set; }

    /// <summary>
    /// 功能性阈值功率
    /// </summary>
    public Power? FTP { get; set; }

    /// <summary>
    /// 活动等级（用于估算训练量）
    /// </summary>
    public ActivityClass? ActivityClass { get; set; }

    /// <summary>
    /// 首选运动类型（如跑步、骑行）
    /// </summary>
    public Sport? Sport { get; set; }

    /// <summary>
    /// 用户语言设置
    /// </summary>
    public Language? Language { get; set; }

    /// <summary>
    /// 用户设置的时区偏移
    /// </summary>
    public TimeSpan? TimezoneOffset { get; set; }
}