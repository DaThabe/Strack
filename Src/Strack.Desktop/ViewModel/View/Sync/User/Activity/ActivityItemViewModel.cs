using CommunityToolkit.Mvvm.Input;
using Strack.Model.Entity.Enum;

namespace Strack.Desktop.ViewModel.View.Sync.Activity;

public partial class ActivityItemViewModel : ObservableObject
{
    /// <summary>
    /// 是否已同步
    /// </summary>
    [ObservableProperty]
    public partial bool IsSynced { get; set; }

    /// <summary>
    /// 活动来源
    /// </summary>
    [ObservableProperty]
    public partial PlatformType Source { get; set; }

    /// <summary>
    /// 活动Id
    /// </summary>
    [ObservableProperty]
    public partial long Id { get; set; }


    /// <summary>
    /// 时间
    /// </summary>
    [ObservableProperty]
    public partial DateTimeOffset Time { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; }

    /// <summary>
    /// 轨迹点数量
    /// </summary>
    [ObservableProperty]
    public partial int RecordPointCount { get; set; }

    /// <summary>
    /// 重新同步
    /// </summary>
    public required IAsyncRelayCommand SyncCommand { get; set; }
}
