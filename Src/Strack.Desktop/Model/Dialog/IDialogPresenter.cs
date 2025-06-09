namespace Strack.Desktop.Model.Dialog;


/// <summary>
/// 弹窗载体
/// </summary>
public interface IDialogPresenter
{
    /// <summary>
    /// 弹窗内容
    /// </summary>
    object? Content { get; set; }
}
