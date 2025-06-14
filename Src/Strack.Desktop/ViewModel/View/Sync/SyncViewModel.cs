using CommunityToolkit.Mvvm.Input;
using Strack.Desktop.ViewModel.View.Sync.Activity;
using Strack.Desktop.ViewModel.View.Sync.User;
using Strack.Model.Entity.Enum;
using Strack.Service;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using XingZhe.Service;

namespace Strack.Desktop.ViewModel.View.Sync;


/// <summary>
/// 同步
/// </summary>
public partial class SyncViewModel(ISyncService syncService) : ObservableObject
{
    /// <summary>
    /// 当前选中用户
    /// </summary>
    [ObservableProperty]
    public partial UserItemViewModel? SelectedUser { get; set; } 

    /// <summary>
    /// 用户列表
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<UserItemViewModel> Users { get; set; } = [];


    public async Task AddXingZheUser(IXingZheClient xingZheClient)
    {
        var user = await xingZheClient.GetUserInfoAsync();
        UserItemViewModel userVm = new()
        {
            Name = user.Name,
            Avatar = new BitmapImage(new Uri(user.AvatarUrl)),
            PullCommand = new AsyncRelayCommand(PullCommand)
        };


        async Task PullCommand()
        {
            var summaries = xingZheClient.GetWorkoutSummaryAsync();

            var vm = summaries.Select(x => new ActivityItemViewModel()
            {
                SyncCommand = new AsyncRelayCommand(SyncCommand),
                IsSynced = false,
                Time = x.StartTime,
                Title = x.Title,
                Source = PlatformType.XingZhe,
                Id = x.Id
            });

            //syncService.GetNotSyncFromXingZheAsync(summaries.Select)

        }

        async Task SyncCommand()
        {

        }
    }
}
