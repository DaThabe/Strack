using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace Strack.Desktop.ViewModel.View.Setting;


public partial class SettingViewModel(IThemeService themeService) : ObservableObject
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
        if (value)
        {
            themeService.SetTheme(ApplicationTheme.Dark);
        }
        else
        {
            themeService.SetTheme(ApplicationTheme.Light);
        }
    }
}
