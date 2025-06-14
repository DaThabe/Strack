using CommunityToolkit.Mvvm.Input;
using Strack.Desktop.ViewModel.View.Sync.Activity;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Strack.Desktop.ViewModel.View.Sync.User;


/// <summary>
/// 用户视图
/// </summary>
public  partial class UserItemViewModel : ObservableObject
{
    /// <summary>
    /// 头像
    /// </summary>
    public BitmapSource? Avatar { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [ObservableProperty]
    public required partial string Name { get; set; }

    /// <summary>
    /// 所有活动
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<ActivityItemViewModel> Activities { get; set; } = [];

    /// <summary>
    /// 拉取命令
    /// </summary>
    public required IAsyncRelayCommand PullCommand { get; set; }
}