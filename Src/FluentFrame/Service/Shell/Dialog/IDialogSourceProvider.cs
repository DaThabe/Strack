using FluentFrame.ViewModel.Shell.Dialog;

namespace FluentFrame.Service.Shell.Dialog;


/// <summary>
/// 弹窗资源
/// </summary>
public interface IDialogSourceProvider
{
    /// <summary>
    /// 内容
    /// </summary>
    DialogViewModel? Content { get; set; }
}