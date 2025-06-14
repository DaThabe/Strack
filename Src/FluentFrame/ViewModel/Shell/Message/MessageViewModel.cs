using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.Shell.Message;



public partial class MessageItemViewModel : ObservableObject
{
    /// <summary>
    /// 等级
    /// </summary>
    [ObservableProperty]
    public required partial InfoBarSeverity Severity { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public required partial string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }
}
