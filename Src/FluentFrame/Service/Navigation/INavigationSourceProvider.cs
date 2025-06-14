using FluentFrame.ViewModel.Shell.Navigation;

namespace FluentFrame.Service;

public interface INavigationSourceProvider
{
    /// <summary>
    /// 页面内容
    /// </summary>
    object? Content { get; set; }

    /// <summary>
    /// 获取目标页类型菜单
    /// </summary>
    /// <returns></returns>
    MenuItemViewModel? GetTargetPageMenuItemViewModel(Type targetPage);
}