using Common.Infrastructure;
using CommunityToolkit.Mvvm.Input;
using FluentFrame.Service.Shell.Message;
using FluentFrame.Service.Shell.Notify;
using FluentFrame.ViewModel.Shell.Message;
using Microsoft.EntityFrameworkCore;
using Strack.Desktop.Extension;
using Strack.Desktop.ViewModel.Page.Activity.Pagination;
using Strack.Desktop.ViewModel.View.Activity.Summary;
using Strack.Model.Entity.Enum;
using Strack.Service;
using Strack.Service.Activity;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UnitsNet;
using Wpf.Ui.Abstractions.Controls;

namespace Strack.Desktop.ViewModel.View.Activity.User;


/// <summary>
/// 用户视图模型
/// </summary>
public partial class UserViewModel(
    IStrackDbService dbService,
    IActivitySyncService activitySyncService,
    IMessageService messageService,
    INotifyService notifyService
    ) : ObservableObject, INavigationAware
{
    #region --属性--

    /// <summary>
    /// 实体Id
    /// </summary>
    [ObservableProperty]
    public partial Guid? EntityId { get; set; }

    /// <summary>
    /// 所属平台
    /// </summary>
    [ObservableProperty]
    public partial PlatformType? PlatformType { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [ObservableProperty]
    public partial BitmapSource? Avatar { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [ObservableProperty]
    public required partial string? Name { get; set; }


    /// <summary>
    /// 所有活动
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<ActivitySummaryViewModel> Activities { get; set; } = [];

    /// <summary>
    /// 活动页码偏移
    /// </summary>
    [ObservableProperty]
    public partial PaginationViewModel? ActivityPagination { get; set; }

    #endregion

    #region --行为--

    //进入
    public async Task OnNavigatedToAsync()
    {
        await ReloadActivityCommand.ExecuteAsync(null);
    }

    //离开
    public async Task OnNavigatedFromAsync()
    {

        await _reloadUserActivityCancellableOperation.CancelAsync();
        await _syncUserActivityCancellableOperation.CancelAsync();
    }

    #endregion

    #region --命令--



    /// <summary>
    /// 同步所有活动
    /// </summary>
    [RelayCommand]
    private async Task SyncActivityAsync()
    {
        if (EntityId is not Guid userEntityId)
        {
            await notifyService.ShowErrorAsync("当前所选用户未设置实体Id", "用户活动加载失败");
            return;
        }

        var cancellationToken = await _syncUserActivityCancellableOperation.StartNew();

        await Task.Run(async () =>
        {
            var userEntity = await dbService.ExecuteAsync(x => x.Users
            .AsNoTracking()
            .Where(x => x.Id == userEntityId)
            .FirstOrDefaultAsync(cancellationToken), cancellationToken);

            if (userEntity == null)
            {
                await notifyService.ShowErrorAsync("未查询到活动实体", "同步失败", cancellation: cancellationToken);
                return;
            }

            //活动同步
            var syncProgress = GetActivitySyncInfoProgress();
            var count = await activitySyncService.SyncAsync(userEntity.Platform, userEntity.ExternalId, syncProgress, cancellationToken);

            //同步完成后 发送通知
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await notifyService.ShowSuccessAsync($"已同步{count}个活动", "同步完成");

            }, DispatcherPriority.Send, cancellationToken);
            
        }, cancellationToken);

        //活动同步进度
        Progress<ActivitySyncInfo> GetActivitySyncInfoProgress()
        {
            MessageItemViewModel message = new(messageService)
            {
                Title = "正在同步活动信息",
                Time = DateTime.Now,
                Appearance = Wpf.Ui.Controls.ControlAppearance.Info
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                messageService.Add(message);
            });

            return new Progress<ActivitySyncInfo>(x =>
            {
                message.Title = $"同步活动 [{x.Completed}/{x.Total}]";
                message.Content = $"{x.Platform}-{x.ActivityId}:{x.State}";
                message.Time = DateTime.Now;
            });
        }
    }

    /// <summary>
    /// 取消同步
    /// </summary>
    [RelayCommand]
    private void CancelSyncActivity()
    {
        _ = _syncUserActivityCancellableOperation.CancelAsync();
    }

    /// <summary>
    /// 加载所有活动
    /// </summary>
    [RelayCommand]
    public async Task ReloadActivityAsync()
    {
        if (EntityId is not Guid userEntityId)
        {
            await notifyService.ShowErrorAsync("当前所选用户未设置实体Id", "用户活动加载失败");
            return;
        }

        var cancellationToken = await _reloadUserActivityCancellableOperation.StartNew();

        if (ActivityPagination == null)
        {
            await LoadActivityPaginationAsync(cancellationToken);
            if (ActivityPagination == null)
            {
                await notifyService.ShowErrorAsync("活动分页加载失败", "用户活动加载失败");
                return;
            }
        }

        Activities.Clear();
        var activityTrackChannel = Channel.CreateUnbounded<ActivitySummaryViewModel>();

        //加载活动和活动轨迹
        await Task.WhenAll(LoadActivityAsync(cancellationToken), LoadActivityTrackAsync(cancellationToken));
        return;


        ///加载活动分页
        async Task LoadActivityPaginationAsync(CancellationToken cancellation)
        {
            var activityCount = await dbService.ExecuteAsync(x => x.Activities
                .AsNoTracking()
                .Where(x => x.UserId == userEntityId)
                .CountAsync(cancellation), cancellation);

            const int size = 10;
            int max = (activityCount + size - 1) / size;

            ActivityPagination = new PaginationViewModel
            {
                Min = 1,
                Max = max,
                Current = 1,
                Size = size,
            };

            //当页码变化时，重新加载活动
            ActivityPagination.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(PaginationViewModel.Current))
                {
                    ReloadActivityCommand.Execute(null);
                }
            };
        }
        //加载活动
        async Task LoadActivityAsync(CancellationToken cancellation)
        {
            var activityEntities = await dbService.ExecuteAsync(x => x.Activities
                .AsNoTracking()
                .Where(x => x.UserId == userEntityId)
                .Include(x => x.Source)
                .Skip((ActivityPagination.Current - 1) * ActivityPagination.Size)
                .Take(ActivityPagination.Size)
                .ToListAsync(cancellation), cancellation);

            foreach (var activityEntity in activityEntities)
            {
                ActivitySummaryViewModel activityVm = new()
                {
                    EntityId = activityEntity.Id,
                    IsSynced = true,

                    Title = activityEntity.Title ?? "活动",
                    Type = activityEntity.Type,

                    Distance = Length.FromMeters(activityEntity.Metrics.Distance.TotalMeters ?? 0),
                    Duration = TimeSpan.FromSeconds(activityEntity.Metrics.Duration.TotalSeconds ?? 0),
                    Time = DateTimeOffset.FromUnixTimeSeconds(activityEntity.FinishUnixTimeSeconds ?? 0),
                };

                Activities.Add(activityVm);
                await Task.Delay(10, cancellation);
                activityTrackChannel.Writer.TryWrite(activityVm);
            }

            activityTrackChannel.Writer.Complete();
        }
        //加载活动轨迹
        async Task LoadActivityTrackAsync(CancellationToken cancellation)
        {
            List<Task> tasks = [];

            await foreach (var item in activityTrackChannel.Reader.ReadAllAsync(cancellation))
            {
                var task = Task.Run(async () =>
                {
                    var points = await dbService.ExecuteAsync(async x =>
                    {
                        var recordEntities = await x.ActivityRecords
                             .AsNoTracking()
                             .Where(x => x.ActivityId == item.EntityId && x.Latitude != null && x.Longitude != null)
                             .OrderBy(x => x.UnixTimeSeconds)
                             .ToListAsync(cancellation);

                        return recordEntities.Select(x => (x.Longitude!.Value, x.Latitude!.Value)).DownSampleByTargetCount(100).ToList();

                    }, cancellation);

                    Application.Current.Dispatcher.Invoke(() => item.TrackPoints = points, DispatcherPriority.Send, cancellation);

                }, cancellation);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
    }

    #endregion

    private readonly CancellableOperation _reloadUserActivityCancellableOperation = new();
    private readonly CancellableOperation _syncUserActivityCancellableOperation = new();
}