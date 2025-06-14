using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace FluentFrame.ViewModel.View.Setting.Category;


/// <summary>
/// 设置类别
/// </summary>
public partial class CategoryViewModel : ObservableObject
{
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty]
    public partial IconElement? Icon { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public required partial string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public  partial object Content { get; set; }
}
