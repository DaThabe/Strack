using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service.Shell.Message;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell.Message;



public partial class MessageItemViewModel(IMessageService messageService) : ObservableObject
{
    /// <summary>
    /// 等级
    /// </summary>
    [ObservableProperty]
    public partial ControlAppearance Appearance { get; set; } = ControlAppearance.Info;

    /// <summary>
    /// 时间
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TimeFormat))]
    public partial DateTime Time { get; set; } = DateTime.Now;

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }


    [RelayCommand]
    private void Close() => messageService.Remove(this);

    public string TimeFormat => Time.ToString("HH:mm:ss");
}