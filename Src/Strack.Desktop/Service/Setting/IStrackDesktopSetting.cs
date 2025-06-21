using Common.Service.Setting;

namespace Strack.Desktop.Service.Setting;


/// <summary>
/// 配置
/// </summary>
public interface IStrackDesktopSetting
{
    /// <summary>
    /// 程序主题
    /// </summary>
    bool IsDarkTheme { get; set; }
}


public class StrackDesktopSetting(ISettingService setting) : IStrackDesktopSetting
{
    public bool IsDarkTheme
    {
        get => setting.Get<bool?>("IsDarkTheme") ?? false;
        set => _ = setting.SetAndSaveAsync("IsDarkTheme", value);
    }
}