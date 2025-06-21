namespace Strack.Desktop.ViewModel.Page.Activity.Pagination;


/// <summary>
/// 分页
/// </summary>
public partial class PaginationViewModel : ObservableObject
{
    /// <summary>
    /// 最小页码
    /// </summary>
    [ObservableProperty]
    public partial int Min { get; set; } = 1;

    /// <summary>
    /// 最大页码
    /// </summary>
    [ObservableProperty]
    public partial int Max { get; set; } = 1;

    /// <summary>
    /// 当前页码
    /// </summary>
    [ObservableProperty]
    public partial int Current { get; set; } = 1;

    /// <summary>
    /// 页面数量
    /// </summary>
    [ObservableProperty]
    public partial int Size { get; set; } = 10;
}