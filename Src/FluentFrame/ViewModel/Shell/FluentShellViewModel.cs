using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service;
using FluentFrame.ViewModel.Shell.Message;
using FluentFrame.ViewModel.Shell.Navigation;
using FluentFrame.ViewModel.Shell.Notify;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell;


public partial class FluentShellViewModel(IServiceProvider services, Service.Navigation.INavigationService navigationService) : ObservableObject, INavigationSourceProvider
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconElement? Icon { get; set; }

    /// <summary>
    /// 是否是夜间主题
    /// </summary>
    [ObservableProperty]
    public partial bool IsDarkTheme { get; set; }    

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = "FluentShell";

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }


    /// <summary>
    /// 导航菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MenuItemViewModel> NavigationMenus { get; set; } = [];

    /// <summary>
    /// 页脚导航菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MenuItemViewModel> NavigationFooterMenus { get; set; } = [];


    /// <summary>
    /// 导航记录
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<RecordItemViewModel> NavigationRecords { get; set; } = [];


    /// <summary>
    /// 消息
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MessageItemViewModel> Messages { get; set; } = [];


    /// <summary>
    /// 通知
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NotifyItemViewModel> Notifies { get; set; } = [];
    
    object? INavigationSourceProvider.Content
    {
        get => Content; 
        set => Content = value;
    }

    MenuItemViewModel? INavigationSourceProvider.GetTargetPageMenuItemViewModel(Type targetPage)
    {
        var result = NavigationMenus.FirstOrDefault(x => x.TargetPageType == targetPage);
        if(result !=  null) return result;

        return NavigationFooterMenus.FirstOrDefault(x => x.TargetPageType == targetPage);
    }


    /// <summary>
    /// 点击导航记录
    /// </summary>
    [RelayCommand]
    private async Task OnNavigationRecordItemClickedAsync(RecordItemViewModel menu)
    {
        await navigationService.NavigationToAsync(menu.TargetPageType);
    }

    /// <summary>
    /// 菜单已被点击
    /// </summary>
    /// <param name="menu"></param>
    [RelayCommand]
    private async Task OnNavigationMenuItemClicked(MenuItemViewModel menu)
    {
        if (menu == _lastSelectedMenuItem) return;

        if (await navigationService.NavigationToAsync(menu.TargetPageType))
        {
            NavigationRecords = [new() { Content = menu.Content, TargetPageType = menu.TargetPageType }];

            _lastSelectedMenuItem?.IsActive = false;
            _lastSelectedMenuItem = menu;
            _lastSelectedMenuItem?.IsActive = true;
        }
        else
        {
            if (menu.IsActive != false) menu.IsActive = false;
            if(_lastSelectedMenuItem?.IsActive != true) _lastSelectedMenuItem?.IsActive = true;
        }
    }

    /// <summary>
    /// 返回按钮被点击
    /// </summary>
    [RelayCommand]
    private async Task OnBackButtonClicked()
    {
        await navigationService.BackAsync();
    }


    /// <summary>
    /// 关闭通知
    /// </summary>
    /// <param name="item"></param>
    [RelayCommand]
    private async Task OnCloseNotifyItemClicked(NotifyItemViewModel item)
    {
        item.IsActive = false;

        await Task.Delay(1000);
        Notifies.Remove(item);
    }


    /// <summary>
    /// 是否开启夜间模式属性变化
    /// </summary>
    partial void OnIsDarkThemeChanged(bool value)
    {
        ApplicationThemeManager.Apply(value ? ApplicationTheme.Dark : ApplicationTheme.Light);
    }


    private MenuItemViewModel? _lastSelectedMenuItem;
}