using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service.Shell.Dialog;
using FluentFrame.Service.Shell.Message;
using FluentFrame.Service.Shell.Notify;
using FluentFrame.ViewModel.Shell.Dialog;
using FluentFrame.ViewModel.Shell.Message;
using FluentFrame.ViewModel.Shell.Notify;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.View;

public partial class DemoViewModel(
    IDialogService dialogService,
    INotifyService notifyService,
    IMessageService messageService
    ) : ObservableObject
{
    public ControlAppearance[] AppearanceItemsSource { get; } = Enum.GetValues<ControlAppearance>();
    public InfoBarSeverity[] SeverityItemsSource { get; } = Enum.GetValues<InfoBarSeverity>();



    [ObservableProperty]
    public partial NotifyItemViewModel NotifyItem { get; set; } = new()
    {
        Title = "通知标题",
        Content = "通知内容",
        Appearance = ControlAppearance.Info,
        Delay = TimeSpan.FromSeconds(3),
    };

    [ObservableProperty]
    public partial DialogViewModel DialogItem { get; set; } = new(dialogService)
    {
        Title = "弹窗标题",
        Content = "弹窗内容",
    };

    [ObservableProperty]
    public partial MessageItemViewModel MessageItem { get; set; } = new(messageService)
    {
        Title = "消息标题",
        Content = "消息内容",
        Appearance =  ControlAppearance.Info,
        Time = DateTime.Now
    };


    [RelayCommand]
    private Task NotifyAsync() => notifyService.ShowAsync(NotifyItem);

    [RelayCommand]
    private void Message() => messageService.Add(MessageItem);

    [RelayCommand]
    private void Dialog() => dialogService.ShowAsync(DialogItem);
}
