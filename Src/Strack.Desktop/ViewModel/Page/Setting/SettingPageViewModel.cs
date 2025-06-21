using CommunityToolkit.Mvvm.ComponentModel;
using Strack.Desktop.Service.Setting;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace Strack.Desktop.ViewModel.View.Setting;


public partial class SettingPageViewModel(IStrackDesktopSetting setting) : ObservableObject
{
    /// <summary>
    /// 是否是暗色主题
    /// </summary>
    [ObservableProperty]
    public partial bool IsDarkTheme { get; set; }


    /// <summary>
    /// 切换主题
    /// </summary>
    partial void OnIsDarkThemeChanged(bool value)
    {
        ApplicationThemeManager.Apply(value ? ApplicationTheme.Light : ApplicationTheme.Light);
        setting.IsDarkTheme = value;
    }
}