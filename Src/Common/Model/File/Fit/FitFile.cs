namespace Common.Model.File.Fit;


/// <summary>
/// Fit 文件信息
/// </summary>
public class FitFile
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public FileInfo? FileInfo { get; set; }

    /// <summary>
    /// 设备信息
    /// </summary>
    public List<DeviceInfo> DeviceInfo { get; set; } = [];

    /// <summary>
    /// 采样点信息
    /// </summary>
    public List<Record> Records { get; set; } = [];

    /// <summary>
    /// 用户信息
    /// </summary>
    public List<UserProfile> UserProfiles { get; set; } = [];
}
