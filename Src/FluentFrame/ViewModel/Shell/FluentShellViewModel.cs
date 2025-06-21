using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service.Shell.Dialog;
using FluentFrame.Service.Shell.Menu;
using FluentFrame.Service.Shell.Message;
using FluentFrame.Service.Shell.Navigation;
using FluentFrame.Service.Shell.Notify;
using FluentFrame.ViewModel.Shell.Dialog;
using FluentFrame.ViewModel.Shell.Message;
using FluentFrame.ViewModel.Shell.Navigation;
using FluentFrame.ViewModel.Shell.Notify;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell;


public partial class FluentShellViewModel : ObservableObject, 
    INavigationSourceProvider,
    IMenuSourceProvider, 
    INotifySourceProvider, 
    IDialogSourceProvider, 
    IMessageSourceProvider
{
    #region --内容属性--

    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconElement? Icon { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = "FluentShell";
    /// <summary>
    /// 是否是夜间主题
    /// </summary>
    [ObservableProperty]
    public partial bool IsDarkTheme { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }
    /// <summary>
    /// 弹窗内容
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ContentOpacity))]
    public partial DialogViewModel? Dialog { get; set; }

    
    /// <summary>
    /// 导航记录
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<RecordItemViewModel> NavigationRecords { get; set; }
    /// <summary>
    /// 导航菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MenuItemViewModel> NavigationMenus { get; set; }
    /// <summary>
    /// 页脚导航菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MenuItemViewModel> NavigationFooterMenus { get; set; }


    /// <summary>
    /// 消息
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MessageItemViewModel> Messages { get; set; }
    /// <summary>
    /// 通知
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NotifyItemViewModel> Notifies { get; set; }

    #endregion

    #region --行为--

    /// <summary>
    /// 内容透明度
    /// </summary>
    public double ContentOpacity => Dialog == null ? 1 : 0.3;

    /// <summary>
    /// 消息数量是否可见
    /// </summary>
    public bool MessageCountVisible => Messages.Count > 0;

    /// <summary>
    /// 消息弹窗是否打开
    /// </summary>
    [ObservableProperty]
    public partial bool MessagePopupOpened { get; set; } = false;


    /// <summary>
    /// 导航返回是否启用
    /// </summary>
    public bool NavigationBackEnable => _pageNavigationService.CanBack;

    /// <summary>
    /// 导航前进是否启用
    /// </summary>
    public bool NavigationForwardEnable => _pageNavigationService.CanForward;


    /// <summary>
    /// 夜间模式属性监听
    /// </summary>
    partial void OnIsDarkThemeChanged(bool value)
    {
        _themeService.SetTheme(value ? ApplicationTheme.Dark : ApplicationTheme.Light);
    }

    /// <summary>
    /// 消息属性见监听
    /// </summary>
    /// <param name="value"></param>
    partial void OnMessagesChanged(ObservableCollection<MessageItemViewModel> value)
    {
        value.CollectionChanged += (sender, e) => 
        OnPropertyChanged(nameof(MessageCountVisible));
    }

    #endregion

    #region --命令--

    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <param name="menu"></param>
    [RelayCommand]
    private async Task NavigationToAsync(Type targetPageType)
    {
        OnPropertyChanged(nameof(NavigationBackEnable));
        OnPropertyChanged(nameof(NavigationForwardEnable));

        await _pageNavigationService.NavigationToAwareAsync(targetPageType);
    }

    /// <summary>
    /// 返回上一个导航
    /// </summary>
    [RelayCommand]
    private async Task NavigationBackAsync()
    {
        OnPropertyChanged(nameof(NavigationBackEnable));
        OnPropertyChanged(nameof(NavigationForwardEnable));

        await _pageNavigationService.BackAwareAsync();
    }

    /// <summary>
    /// 返回前一个导航
    /// </summary>
    [RelayCommand]
    private async Task NavigationForwardAsync()
    {
        OnPropertyChanged(nameof(NavigationBackEnable));
        OnPropertyChanged(nameof(NavigationForwardEnable));

        await _pageNavigationService.ForwardAwareAsync();
    }



    /// <summary>
    /// 关闭通知
    /// </summary>
    /// <param name="item"></param>
    [RelayCommand]
    private Task CloseNotifyAsync(NotifyItemViewModel item)
    {
        return _notifyService.CloseAsync(item);
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    /// <param name="item"></param>
    [RelayCommand]
    private void RemoveMessageAsync(MessageItemViewModel item)
    {
        _messageService.Remove(item);
    }

    /// <summary>
    /// 切换主题
    /// </summary>
    [RelayCommand]
    private void ChangeTheme() => IsDarkTheme = !IsDarkTheme;


    #endregion



    public FluentShellViewModel(
        IDialogService dialogService,
        INotifyService notifyService,
        IMessageService messageService,
        IPageNavigationService pageNavigationService,
        IMenuService menuService,
        IThemeService themeService
    )
    {
        _themeService = themeService;

        _notifyService = notifyService;
        _notifyService.SetSourceProvider(this);

        _messageService = messageService;
        _messageService.SetSourceProvider(this);

        _pageNavigationService = pageNavigationService;
        _pageNavigationService.SetSourceProvider(this);

        _menuService = menuService;
        _menuService.SetSourceProvider(this);

        _dialogService = dialogService;
        _dialogService.SetSourceProvider(this);


        NavigationRecords = [];
        NavigationMenus = [];
        NavigationFooterMenus = [];

        Messages = [];
        Notifies = [];
    }


    private readonly INotifyService _notifyService;
    private readonly IMessageService _messageService;
    private readonly IPageNavigationService _pageNavigationService;
    private readonly IMenuService _menuService;
    private readonly IThemeService _themeService;
    private readonly IDialogService _dialogService;

    ObservableCollection<MessageItemViewModel> IMessageSourceProvider.ItemsSource
    {
        get => Messages;
    }
    DialogViewModel? IDialogSourceProvider.Content
    {
        get => Dialog;
        set => Dialog = value;
    }
    ObservableCollection<MenuItemViewModel> IMenuSourceProvider.ItemsSource
    {
        get => NavigationMenus;
    }
    ObservableCollection<MenuItemViewModel> IMenuSourceProvider.FooterItemsSource
    {
        get => NavigationFooterMenus;
    }
    ObservableCollection<NotifyItemViewModel> INotifySourceProvider.ItemsSource
    {
        get => Notifies;
    }
}