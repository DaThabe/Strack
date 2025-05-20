using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Strack.Desktop.Extension;
using Strack.Desktop.ViewModel.View.Dashboard.Activity;
using Strack.Extension;
using Strack.External.Xingzhe;
using Strack.External.Xingzhe.Model;
using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Threading;
using UnitsNet;

namespace Strack.Desktop.ViewModel.View.Dashboard;

public partial class DashboardViewModel(IXingzheApi xingzheApi) : ObservableObject
{
    /// <summary>
    /// 数据统计开始时间
    /// </summary>
    [ObservableProperty]
    public partial DateTime BeginTime { get; set; }

    /// <summary>
    /// 数据统计结束时间
    /// </summary>
    [ObservableProperty]
    public partial DateTime EndTime { get; set; }

    /// <summary>
    /// 活动集合
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<ActivityViewModel> Activities { get; set; } = [];

    /// <summary>
    /// 活动统计集合
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<ActivityStatistics> Statistics { get; set; } = [];

    [RelayCommand]
    private async Task CalcStatisticsAsync()
    {
        Activities.Clear();
        var results = await xingzheApi.GetActivitySummariesAsync(6117373);
        int count = 0;
        var trackDownloadSemaphore = new SemaphoreSlim(1);

        foreach (var i in results)
        {
            var item = new ActivityViewModel()
            {
                Distance = i.Distance,
                Duration = i.Duration,
                Timestamp = i.Timestamp,
                Track = null,
                Type = i.Type.ToActivityType()
            };

            Activities.Add(item);
            await Task.Delay(10);

            var itemId = i.Id;
            _ = Task.Run(async () =>
            {
                await trackDownloadSemaphore.WaitAsync();

                try
                {
                    var trackInfo = await xingzheApi.GetTrackPointSummaryAsync(6117373, itemId);

                    var trackData = trackInfo.Select(x => (x.Longitude, x.Latitude))
                            .NormalizeToOrigin()
                            .ScaleToFit(800)
                            .RemoveColinearPoints(0.01)
                            .RoundCoordinates(3)
                            .ToGeometry();

                    item.Track = trackData;
                }
                catch
                {
                    Activities.Remove(item);
                }
                finally
                {
                    trackDownloadSemaphore.Release();
                }
            });

            if (count++ > 10) break;
        }


        Statistics = Activities.GroupBy(x => x.Type).Select(x => new ActivityStatistics()
        {
            Type = x.Key,
            Distance = Length.FromKilometers(x.Sum(x => x.Distance.Kilometers)),
            Duration = TimeSpan.FromMinutes(x.Sum(x => x.Duration.TotalMinutes))

        }).OrderByDescending(x=>x.Distance.Meters).ToObservableCollection();
    }
}
