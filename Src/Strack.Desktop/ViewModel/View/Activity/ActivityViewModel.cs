using CommunityToolkit.Mvvm.ComponentModel;
using Strack.Model;
using System.Globalization;
using System.Windows.Media;

namespace Strack.Desktop.ViewModel.View.Activity;


/// <summary>
/// 活动信息
/// </summary>
public partial class ActivityViewModel : ActivityStatistics
{
    /// <summary>
    /// 路径
    /// </summary>
    [ObservableProperty]
    public partial Geometry Track { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TimestampFormatted))]
    public partial DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// 日期格式化
    /// </summary>
    public string TimestampFormatted => Timestamp.LocalDateTime.ToString("MM-dd - ddd", CultureInfo.CurrentCulture);
}
