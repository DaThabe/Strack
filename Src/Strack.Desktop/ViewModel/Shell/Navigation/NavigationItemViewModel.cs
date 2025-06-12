using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Controls;

namespace Strack.Desktop.ViewModel.Shell.Navigation;


/// <summary>
/// 导航元素
/// </summary>
public partial class NavigationItemViewModel : ObservableObject
{
    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public required partial string Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconElement? Icon { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty] 
    public required partial object Content { get; set; }
}



internal static class NavigationInfo
{
    public static NavigationItemViewModel Error(object content, string caption = "错误", string? title = null) => new()
    {
        Title = title ?? "",
        Icon = new SymbolIcon(SymbolRegular.AccessTime20),
        Content = new InfoBar()
        {
            IsClosable = false,
            IsOpen = true,
            Severity = InfoBarSeverity.Error,
            Content = content,
            Title = caption
        }
    };

    public static NavigationItemViewModel Warning(object content, string caption = "警告", string? title = null) => new()
    {
        Title = "Error",
        Icon = new SymbolIcon(SymbolRegular.AccessTime20),
        Content = new InfoBar()
        {
            IsClosable = false,
            IsOpen = true,
            Severity = InfoBarSeverity.Warning,
            Content = content,
            Title = caption
        }
    };
    public static NavigationItemViewModel Success(object content, string caption = "成功", string? title = null) => new()
    {
        Title = title ?? "",
        Icon = new SymbolIcon(SymbolRegular.AccessTime20),
        Content = new InfoBar()
        {
            IsClosable = false,
            IsOpen = true,
            Severity = InfoBarSeverity.Success,
            Content = content,
            Title = caption
        }
    };

    public static NavigationItemViewModel Info(object content, string caption = "消息", string? title = null) => new()
    {
        Title = title ?? "",
        Icon = new SymbolIcon(SymbolRegular.AccessTime20),
        Content = new InfoBar()
        {
            IsClosable = false,
            IsOpen = true,
            Severity = InfoBarSeverity.Informational,
            Content = content,
            Title = caption
        }
    };
}
