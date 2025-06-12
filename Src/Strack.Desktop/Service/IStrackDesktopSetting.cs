using Common.Service.Setting;
using Wpf.Ui.Appearance;

namespace Strack.Desktop.Service;


/// <summary>
/// 配置
/// </summary>
public interface IStrackDesktopSetting
{
    /// <summary>
    /// 程序主题
    /// </summary>
    ApplicationTheme Theme { get; set; }
}


public class StrackDesktopSetting(ISettingService setting) : IStrackDesktopSetting
{
    public ApplicationTheme Theme
    {
        get => setting.Get<ApplicationTheme?>("ApplicationTheme") ?? ApplicationTheme.Light;
        set => _ = setting.SetAndSaveAsync("ApplicationTheme", value);
    }
}